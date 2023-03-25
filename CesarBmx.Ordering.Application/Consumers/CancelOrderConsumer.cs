using CesarBmx.Shared.Messaging.Ordering.Commands;
using CesarBmx.Shared.Messaging.Ordering.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CesarBmx.Ordering.Application.Consumers
{
    public class CancelOrderConsumer : IConsumer<CancelOrder>
    {
        readonly ILogger<CancelOrderConsumer> _logger;

        public CancelOrderConsumer(ILogger<CancelOrderConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CancelOrder> context)
        {
            _logger.LogInformation("Cancel order");

            var orderCancelled = new OrderCancelled { OrderId = context.Message.OrderId };
            await context.Publish(orderCancelled);

            await context.RespondAsync(orderCancelled);
        }
    }

}
