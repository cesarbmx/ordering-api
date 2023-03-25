using CesarBmx.Ordering.Domain.Types;
using System;
using System.ComponentModel.DataAnnotations;


namespace CesarBmx.Ordering.Application.Requests
{
    public class PlaceOrder
    {
        [Required] public Guid OrderId { get; private set; }
        [Required] public string UserId { get; private set; }
        [Required] public string CurrencyId { get; private set; }
        [Required] public decimal Price { get; private set; }
        [Required] public OrderType OrderType { get; private set; }
        [Required] public decimal Quantity { get; private set; }
        [Required] public OrderStatus OrderStatus { get; private set; }
        [Required] public DateTime? PlacedAt { get; private set; }
        [Required] public DateTime? FilledAt { get; private set; }
        [Required] public DateTime? CancelledAt { get; private set; }
    }
}
