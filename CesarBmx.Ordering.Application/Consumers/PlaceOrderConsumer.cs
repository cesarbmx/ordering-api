using CesarBmx.Ordering.Domain.Builders;
using CesarBmx.Shared.Messaging.Ordering.Commands;
using CesarBmx.Shared.Messaging.Ordering.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using AutoMapper;
using CesarBmx.Ordering.Persistence.Contexts;
using CesarBmx.Ordering.Application.Services;
using CesarBmx.Shared.Messaging.Notification.Events;

namespace CesarBmx.Ordering.Application.Consumers
{
    public class PlaceOrderConsumer : IConsumer<SubmitOrder>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<PlaceOrderConsumer> _logger;
        private readonly ActivitySource _activitySource;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly OrderService _orderService;

        public PlaceOrderConsumer(
            MainDbContext mainDbContext,
            IMapper mapper,
            ILogger<PlaceOrderConsumer> logger,
            ActivitySource activitySource,
            IPublishEndpoint publishEndpoint,
            OrderService orderService)
        {
            _mainDbContext = mainDbContext;
            _mapper = mapper;
            _logger = logger;
            _activitySource = activitySource;
            _publishEndpoint = publishEndpoint;
            _orderService = orderService;
        }

        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            try
            {
                // Start watch
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Start span
                using var span = _activitySource.StartActivity(nameof(SubmitOrder));

                // New order
                var newOrder = OrderBuilder.BuildOrder(context.Message, DateTime.UtcNow);

                // Add
                await _mainDbContext.Orders.AddAsync(newOrder);

                // Save
                await _mainDbContext.SaveChangesAsync();

                // Event
                var orderPlaced = _mapper.Map<OrderPlaced>(newOrder);

                // Publish event
                await _publishEndpoint.Publish(orderPlaced);

                // Response
                await context.RespondAsync(orderPlaced);

                // Stop watch
                stopwatch.Stop();

                // Log
                _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", nameof(OrderPlaced), Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);
            }
            catch(Exception ex)
            {
                // Log
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
