using System;
using CesarBmx.Ordering.Application.Requests;
using CesarBmx.Ordering.Domain.Models;

namespace CesarBmx.Ordering.Domain.Builders
{
    public static class OrderFactory
    {
        public static Order CreateOrder(this PlaceOrder placeOrder, DateTime createdAt)
        {
            var order = new Order(
                placeOrder.OrderId,
                placeOrder.UserId,
                placeOrder.CurrencyId,
                placeOrder.Price,
                placeOrder.Quantity,
                placeOrder.OrderType,
                createdAt
                );

            return order;
        }
            
    }
}
