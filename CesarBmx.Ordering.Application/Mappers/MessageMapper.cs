using AutoMapper;
using CesarBmx.Ordering.Domain.Models;

namespace CesarBmx.Ordering.Application.Mappers
{
    public class MessageMapper : Profile
    {
        public MessageMapper()
        {
            CreateMap<Message, Responses.Message>();
        }
    }
}
