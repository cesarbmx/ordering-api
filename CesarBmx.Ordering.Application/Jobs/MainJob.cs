using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CesarBmx.Ordering.Application.Services;
using Hangfire;
using Microsoft.Extensions.Logging;


namespace CesarBmx.Ordering.Application.Jobs
{
    public class MainJob
    {
        private readonly OrderService _orderService;
        private readonly ILogger<MainJob> _logger;
        private readonly ActivitySource _activitySource;

        public MainJob(
            OrderService orderService,
            ILogger<MainJob> logger,
            ActivitySource activitySource)
        {
            _orderService = orderService;
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
                //await _orderService.RetryPlacingOrders();

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