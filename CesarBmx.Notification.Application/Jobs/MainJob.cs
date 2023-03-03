using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CesarBmx.Notification.Application.Services;
using Hangfire;
using Microsoft.Extensions.Logging;


namespace CesarBmx.Notification.Application.Jobs
{
    public class MainJob
    {
        private readonly MessageService _messageService;
        private readonly ILogger<MainJob> _logger;
        private readonly ActivitySource _activitySource;

        public MainJob(
            MessageService messageService,
            ILogger<MainJob> logger,
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
                // Start watch
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Start span
                using var span = _activitySource.StartActivity(nameof(MainJob));

                // Main job
                await _messageService.SendTelegramMessages();

                // Stop watch
                stopwatch.Stop();

                // Log
                _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", "MainJobFinished", Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);
            }
            catch (Exception ex)
            {
                // Log
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}