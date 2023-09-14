using AutoMapper;
using CesarBmx.Ordering.Domain.Models;
using CesarBmx.Shared.Messaging.Ordering.Commands;
using CesarBmx.Shared.Messaging.Ordering.Events;

namespace CesarBmx.Ordering.Application.Mappers
{
    public class OrderMapper : Profile
    {
        public OrderMapper()
        {
            // Model to Response
            CreateMap<Order, Responses.Order>();

            // Model to Event
            CreateMap<Order, OrderPlaced>();
            CreateMap<Order, OrderCancelled>();
            CreateMap<Order, OrderFilled>();

            // Model to Command
            CreateMap<Order, PlaceOrder>();
            CreateMap<Order, CancelOrder>();

            // Command to Model
            CreateMap<PlaceOrder, Order>();
        }
    }
}
