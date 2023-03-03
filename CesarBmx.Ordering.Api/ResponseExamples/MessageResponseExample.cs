using CesarBmx.Ordering.Application.FakeResponses;
using CesarBmx.Ordering.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace CesarBmx.Ordering.Api.ResponseExamples
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