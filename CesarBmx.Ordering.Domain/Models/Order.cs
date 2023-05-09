using System;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.Ordering.Domain.Types;


namespace CesarBmx.Ordering.Domain.Models
{
    public class Order 
    {
        public Guid OrderId { get; private set; }
        public string UserId { get; private set; }
        public string CurrencyId { get; private set; }
        public decimal Price { get; private set; }
        public OrderType OrderType { get; private set; }
        public decimal Quantity { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public DateTime? Submitted { get; private set; }
        public DateTime? PlacedAt { get; private set; }
        public DateTime? FilledAt { get; private set; }
        public DateTime? CancelledAt { get; private set; }
        public DateTime? ExpiredAt { get; private set; }

        public Order() { }
        public Order(
            Guid orderId,
            string userId,
            string currencyId,
            decimal price,
            decimal quantity,
            OrderType orderType,
            DateTime createdAt)
        {
            OrderId = orderId;
            UserId = userId;
            CurrencyId = currencyId;
            Price = price;
            Quantity = quantity;
            OrderType = orderType;
            OrderStatus = OrderStatus.PLACED;
            PlacedAt = createdAt;
        }

        public Order MarkAsPlaced()
        {
            OrderStatus = OrderStatus.PLACED;
            PlacedAt = DateTime.UtcNow.StripSeconds();

            return this;
        }
        public Order MarkAsFilled()
        {
            OrderStatus = OrderStatus.FILLED;
            FilledAt = DateTime.UtcNow.StripSeconds();

            return this;
        }
        public Order MarkAsCancelled()
        {
            OrderStatus = OrderStatus.CANCELLED;
            CancelledAt = DateTime.UtcNow.StripSeconds();

            return this;
        }
    }
}
