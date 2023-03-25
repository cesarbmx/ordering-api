using System;
using CesarBmx.Ordering.Domain.Types;

namespace CesarBmx.Ordering.Application.Responses
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public string UserId { get; set; }
        public string CurrencyId { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public OrderType OrderType { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime? FilledAt { get; set; }
    }
}
