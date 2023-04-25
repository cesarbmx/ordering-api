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
        private readonly IPublishEndpoint _publishEndpoint;

        public CancelOrderConsumer(
            MainDbContext mainDbContext,
            IMapper mapper,
            ILogger<CancelOrderConsumer> logger,
            ActivitySource activitySource,
            IPublishEndpoint publishEndpoint)
        {
            _mainDbContext = mainDbContext;
            _mapper = mapper;
            _logger = logger;
            _activitySource = activitySource;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<CancelOrder> context)
        {
            try
            {
                // Start watch
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Start span
                using var span = _activitySource.StartActivity(nameof(SubmitOrder));

                // Get order
                var order = await _mainDbContext.Orders.FirstOrDefaultAsync(x=>x.OrderId == context.Message.OrderId);

                // Mark as cancelled
                order.MarkAsCancelled();

                // Save
                await _mainDbContext.SaveChangesAsync();

                // Event
                var orderCancelled = _mapper.Map<OrderCancelled>(order);

                // Publish event
                await _publishEndpoint.Publish(orderCancelled);

                // Response
                await context.RespondAsync(orderCancelled);

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
