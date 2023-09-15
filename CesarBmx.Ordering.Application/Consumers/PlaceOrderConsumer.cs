 using CesarBmx.Shared.Messaging.Ordering.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using AutoMapper;
using CesarBmx.Ordering.Persistence.Contexts;
using CesarBmx.Shared.Messaging.Ordering.Commands;
using CesarBmx.Ordering.Domain.Models;
using CesarBmx.Shared.Messaging.Notification.Commands;
using CesarBmx.Shared.Messaging.Notification.Types;
using CesarBmx.Ordering.Domain.Builders;

namespace CesarBmx.Ordering.Application.Consumers
{
    public class PlaceOrderConsumer : IConsumer<PlaceOrder>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<PlaceOrderConsumer> _logger;
        private readonly ActivitySource _activitySource;
        private readonly IBus _bus;

        public PlaceOrderConsumer(
            MainDbContext mainDbContext,
            IMapper mapper,
            ILogger<PlaceOrderConsumer> logger,
            ActivitySource activitySource,
            IBus bus)
        {
            _mainDbContext = mainDbContext;
            _mapper = mapper;
            _logger = logger;
            _activitySource = activitySource;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<PlaceOrder> context)
        {
            try
            {
                // Start watch
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Start span
                using var span = _activitySource.StartActivity(nameof(OrderPlaced));

                // Command
                var placeOrder = context.Message;

                // TODO: Place order on Binance

                // Create order
                var order = _mapper.Map<Order>(placeOrder);

                // Add order
                await _mainDbContext.Orders.AddAsync(order);

                 // Event
                 var orderPlaced = _mapper.Map<OrderPlaced>(order);

                // Command
                var sendNotification = order.BuildSendMessage();

                // Send message
                await _bus.Send(sendNotification);

                // Response
                await context.RespondAsync(orderPlaced);

                // Publish
                if (context.IsResponseAccepted<OrderPlaced>())
                    await context.Publish(orderPlaced);

                // Save
                await _mainDbContext.SaveChangesAsync();

                // Stop watch
                stopwatch.Stop();

                // Log
                _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", nameof(OrderPlaced), Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);               
            }
            catch (Exception ex)
            {
                // Log
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
