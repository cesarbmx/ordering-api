using CesarBmx.Ordering.Domain.Builders;
using CesarBmx.Ordering.Persistence.Contexts;
using CesarBmx.Shared.Messaging.Ordering.Commands;
using CesarBmx.Shared.Messaging.Ordering.Events;
using MassTransit;
using MassTransit.Transports;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace CesarBmx.Ordering.Application.Consumers
{
    public class CancelOrderConsumer : IConsumer<CancelOrder>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CancelOrderConsumer> _logger;
        private readonly ActivitySource _activitySource;

        public CancelOrderConsumer(
            MainDbContext mainDbContext,
            IMapper mapper,
            ILogger<CancelOrderConsumer> logger,
            ActivitySource activitySource)
        {
            _mainDbContext = mainDbContext;
            _mapper = mapper;
            _logger = logger;
            _activitySource = activitySource;
        }

        public async Task Consume(ConsumeContext<CancelOrder> context)
        {
            try
            {               

                // Start watch
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Start span
                using var span = _activitySource.StartActivity(nameof(CancelOrder));

                // Get order
                var order = await _mainDbContext.Orders.FirstOrDefaultAsync(x=>x.OrderId == context.Message.OrderId);

                // Mark as cancelled
                order.MarkAsCancelled();
              
                // Event
                var orderCancelled = _mapper.Map<OrderCancelled>(order);

                // Response
                await context.RespondAsync(orderCancelled);

                // Publish
                if (context.IsResponseAccepted<OrderCancelled>()) 
                    await context.Publish(orderCancelled);

                // Either respond or publish
                if (context.IsResponseAccepted<OrderCancelled>())
                {
                    // Response
                    await context.RespondAsync(orderCancelled);
                }
                else
                {
                    // Publish
                    await context.Publish(orderCancelled);
                }

                // Save
                await _mainDbContext.SaveChangesAsync();

                // Stop watch
                stopwatch.Stop();

                // Log
                _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", nameof(OrderCancelled), Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);
              
            }
            catch (Exception ex)
            {
                // Log
                _logger.LogError(ex, ex.Message);
            }
        }
    }

}
