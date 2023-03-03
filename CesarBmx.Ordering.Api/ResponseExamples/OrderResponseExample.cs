using CesarBmx.Ordering.Application.FakeResponses;
using CesarBmx.Ordering.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace CesarBmx.Ordering.Api.ResponseExamples
{
    public class OrderResponseExample : IExamplesProvider<Order>
    {
        public Order GetExamples()
        {
            return FakeOrder.GetFake_Master();
        }
    }
    public class NotificationListResponseExample : IExamplesProvider<List<Order>>
    {
        public List<Order> GetExamples()
        {
            return FakeOrder.GetFake_List();
        }
    }
}