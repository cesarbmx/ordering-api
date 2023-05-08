 using CesarBmx.Shared.Messaging.Ordering.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using AutoMapper;
using CesarBmx.Ordering.Persistence.Contexts;
using CesarBmx.Ordering.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace CesarBmx.Ordering.Application.Consumers
{
    public class OrderFilledConsumer : IConsumer<OrderFilled>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderFilledConsumer> _logger;
        private readonly ActivitySource _activitySource;

        public OrderFilledConsumer(
            MainDbContext mainDbContext,
            IMapper mapper,
            ILogger<OrderFilledConsumer> logger,
            ActivitySource activitySource)
        {
            _mainDbContext = mainDbContext;
            _mapper = mapper;
            _logger = logger;
            _activitySource = activitySource;
        }

        public async Task Consume(ConsumeContext<OrderFilled> context)
        {
            try
            {
                // Start watch
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Start span
                using var span = _activitySource.StartActivity(nameof(OrderFilled));

                // Event
                var orderSubmitted = context.Message;

                // TODO: Place order on Binance

                // Get order
                var order = await _mainDbContext.Orders.FirstOrDefaultAsync(x => x.OrderId == orderSubmitted.OrderId);

                // Mark as Filled
                order.MarkAsFilled();

                // Save
                await _mainDbContext.SaveChangesAsync();                        

                // Stop watch
                stopwatch.Stop();

                // Log
                _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", nameof(OrderFilled), Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);
            }
            catch(Exception ex)
            {
                // Log
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
