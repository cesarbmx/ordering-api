using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CesarBmx.Ordering.Application.Services;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace CesarBmx.Ordering.Application.Jobs
{
    public class SendTelgramNotificationsJob
    {
        private readonly MessageService _messageService;
        private readonly ILogger<SendWhatsappNotificationsJob> _logger;
        private readonly ActivitySource _activitySource;

        public SendTelgramNotificationsJob(
            MessageService messageService,
            ILogger<SendWhatsappNotificationsJob> logger,
            ActivitySource activitySource)
        {
            _messageService = messageService;
            _logger = logger;
            _activitySource = activitySource;
        }

        [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task Run()
        {
            try
            {
                // Start span
                using var span = _activitySource.StartActivity(nameof(SendTelgramNotificationsJob));

                // Send telegram notifications
                await _messageService.SendTelegramMessages();
            }
            catch (Exception ex)
            {
                // Log
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}