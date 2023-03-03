using CesarBmx.Shared.Messaging.Notification.Commands;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CesarBmx.Ordering.Application.Consumers
{
    public class SendMessageConsumer : IConsumer<SendMessage>
    {
        readonly ILogger<SendMessageConsumer> _logger;

        public SendMessageConsumer(ILogger<SendMessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<SendMessage> context)
        {
            _logger.LogInformation("Send message " + context.Message.Text);

            return Task.CompletedTask;
        }
    }

}
