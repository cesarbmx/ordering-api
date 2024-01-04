 using CesarBmx.Shared.Messaging.Ordering.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using AutoMapper;
using CesarBmx.Ordering.Persistence.Contexts;
using CesarBmx.Shared.Messaging.Ordering.Commands;
using Microsoft.EntityFrameworkCore;

namespace CesarBmx.Ordering.Application.Consumers
{
    public class PlaceOrderConsumer : IConsumer<PlaceOrder>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<PlaceOrderConsumer> _logger;
        private readonly ActivitySource _activitySource;

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
        }

        public async Task Consume(ConsumeContext<PlaceOrder> context)
        {
            try
            {
                // Start watch
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Start span
                using var span = _activitySource.StartActivity(nameof(PlaceOrderConsumer));

                // Command
                var placeOrder = context.Message;

                // TODO: Place order on Binance

                // Check if it already exists
                var order = await _mainDbContext.Orders.SingleOrDefaultAsync(x => x.OrderId == placeOrder.OrderId);

                // Return if it exists
                if (order != null) return;

                // Create order
                order = OrderBuilder.BuildOrder(placeOrder, DateTime.UtcNow);

                // Add order
                await _mainDbContext.Orders.AddAsync(order);

                // Command
                var sendNotification = order.BuildSendMessage();

                // Send message
                await context.Send(sendNotification);

                // Event
                var orderPlaced = _mapper.Map<OrderPlaced>(order);

                // Either respond or publish
                if (context.IsResponseAccepted<OrderPlaced>())
                {
                    // Response
                    await context.RespondAsync(orderPlaced);
                }
                else
                {
                    // Publish
                    await context.Publish(orderPlaced);
                }                 

                // Save
                await _mainDbContext.SaveChangesAsync();

                // Stop watch
                stopwatch.Stop();

                // Log
                _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", nameof(PlaceOrderConsumer), Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);               
            }
            catch (Exception ex)
            {
                // Log
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
