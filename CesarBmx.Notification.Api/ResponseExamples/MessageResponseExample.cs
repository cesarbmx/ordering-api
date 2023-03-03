using CesarBmx.Notification.Application.FakeResponses;
using CesarBmx.Notification.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace CesarBmx.Notification.Api.ResponseExamples
{
    public class MessageResponseExample : IExamplesProvider<Message>
    {
        public Message GetExamples()
        {
            return FakeMessage.GetFake_Master();
        }
    }
    public class NotificationListResponseExample : IExamplesProvider<List<Message>>
    {
        public List<Message> GetExamples()
        {
            return FakeMessage.GetFake_List();
        }
    }
}