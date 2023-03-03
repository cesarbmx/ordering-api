using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CesarBmx.Shared.Application.Exceptions;
using CesarBmx.Ordering.Domain.Expressions;
using CesarBmx.Ordering.Application.Messages;
using CesarBmx.Ordering.Application.Settings;
using CesarBmx.Ordering.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;

namespace CesarBmx.Ordering.Application.Services
{
    public class MessageService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly ILogger<MessageService> _logger;
        private readonly ActivitySource _activitySource;

        public MessageService(
            MainDbContext mainDbContext,
            IMapper mapper,
            AppSettings appSettings,
            ILogger<MessageService> logger,
            ActivitySource activitySource)
        {
            _mainDbContext = mainDbContext;
            _mapper = mapper;
            _appSettings = appSettings;
            _logger = logger;
            _activitySource = activitySource;
        }

        public async Task<List<Responses.Message>> GetMessages(string userId)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetMessages));       

            // Get user
            var notifications = await _mainDbContext.Messages
                .Where(x => x.UserId == userId).ToListAsync();

            // Response
            var response = _mapper.Map<List<Responses.Message>>(notifications);

            // Return
            return response;
        }
        public async Task<Responses.Message> GetMessage(Guid messageId)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetMessage));

            // Get notification
            var notification = await _mainDbContext.Messages.FindAsync(messageId);

            // Throw NotFound if the currency does not exist
            if (notification == null) throw new NotFoundException(NotificationMessage.NotificationNotFound);

            // Response
            var response = _mapper.Map<Responses.Message>(notification);

            // Return
            return response;
        }

        public async Task SendTelegramMessages()
        {
            // Start watch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Start span
            using var span = _activitySource.StartActivity(nameof(SendTelegramMessages));

            // Get pending notifications
            var messages = await _mainDbContext.Messages.Where(NotificationExpression.PendingNotification()).ToListAsync();

            // Connect
            var apiToken = _appSettings.TelegramApiToken;
            var bot = new TelegramBotClient(apiToken);

            // For each notification
            var count = 0;
            var failedCount = 0;
            foreach (var message in messages)
            {
                try
                {
                    // Send telegram
                    await bot.SendTextMessageAsync("@crypto_watcher_official", message.Text);

                    // Mark notification as sent
                    message.MarkAsSent();

                    // Update notification
                    _mainDbContext.Messages.Update(message);

                    // Save
                    await _mainDbContext.SaveChangesAsync();

                    // Count
                    count++;
                }
                catch (Exception ex)
                {
                    // Log
                    _logger.LogError(ex, ex.Message);
                    failedCount++;
                }
            }

            // Stop watch
            stopwatch.Stop();

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@Count}, {@FailedCount}, {@ExecutionTime}", "TelegramNotificationsSent", Guid.NewGuid(), count, failedCount, stopwatch.Elapsed.TotalSeconds);
        }
        public async Task SendWhatsappNotifications()
        {
            // Start watch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Start span
            using var span = _activitySource.StartActivity(nameof(SendWhatsappNotifications));

            // Get pending messages
            var pendingMessages = await _mainDbContext.Messages.Where(NotificationExpression.PendingNotification()).ToListAsync();

            // If there are pending notifications
            if (pendingMessages.Count > 0)
            {
                // Connect
                TwilioClient.Init(
                    Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID"),
                    Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN")
                );

                // For each notification
                var count = 0;
                var failedCount = 0;
                foreach (var pendingMessage in pendingMessages)
                {
                    try
                    {
                        // Send whatsapp
                        MessageResource.Create(
                            from: new PhoneNumber("whatsapp:" + pendingMessage.PhoneNumber),
                            to: new PhoneNumber("whatsapp:" + "+34666666666"),
                            body: pendingMessage.Text
                        );
                        pendingMessage.MarkAsSent();
                        count++;
                    }
                    catch (Exception ex)
                    {
                        // Log
                        _logger.LogError(ex, ex.Message);
                        failedCount++;
                    }
                }

                // Save
                await _mainDbContext.SaveChangesAsync();

                // Stop watch
                stopwatch.Stop();

                // Log
                _logger.LogInformation("{@Event}, {@Id}, {@Count}, {@FailedCount}, {@ExecutionTime}", "WhatsappNotificationsSent", Guid.NewGuid(), count, failedCount, stopwatch.Elapsed.TotalSeconds);
            }
        }
    }
}
