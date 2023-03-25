using CesarBmx.Ordering.Application.Requests;
using CesarBmx.Shared.Messaging.Ordering.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CesarBmx.Ordering.Application.Consumers
{
    public class PlaceOrderConsumer : IConsumer<PlaceOrder>
    {
        readonly ILogger<PlaceOrderConsumer> _logger;

        public PlaceOrderConsumer(ILogger<PlaceOrderConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<PlaceOrder> context)
        {
            _logger.LogInformation("Add order");

            // Publish OrderAdded
            var orderAdded = new OrderPlaced { OrderId = context.Message.OrderId };
            context.Publish(orderAdded);

            return Task.CompletedTask;
        }
    }
}
