using AutoMapper;
using CesarBmx.Ordering.Domain.Models;

namespace CesarBmx.Ordering.Application.Mappers
{
    public class OrderMapper : Profile
    {
        public OrderMapper()
        {
            CreateMap<Order, Responses.Order>();
        }
    }
}
