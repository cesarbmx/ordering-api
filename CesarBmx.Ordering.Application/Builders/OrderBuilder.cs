using System;
using CesarBmx.Ordering.Application.Requests;
using CesarBmx.Ordering.Domain.Models;
using CesarBmx.Ordering.Domain.Types;
using Command = CesarBmx.Shared.Messaging.Ordering.Commands;
using CommandType = CesarBmx.Shared.Messaging.Ordering.Types;

namespace CesarBmx.Ordering.Domain.Builders
{
    public static class OrderBuilder
    {
        public static Order BuildOrder(this SubmitOrder submitOrder, DateTime createdAt)
        {
            var order = new Order(
                Guid.NewGuid(),
                submitOrder.UserId,
                submitOrder.CurrencyId,
                submitOrder.Price,
                submitOrder.Quantity,
                submitOrder.OrderType,
                createdAt
                );

            return order;
        }
        public static Order BuildOrder(this PlaceOrder placeOrder, DateTime createdAt)
        {
            var order = new Order(
                Guid.NewGuid(),
                placeOrder.UserId,
                placeOrder.CurrencyId,
                placeOrder.Price,
                placeOrder.Quantity,
                placeOrder.OrderType,
                createdAt
                );

            return order;
        }
        public static Order BuildOrder(this Command.SubmitOrder submitOrder, DateTime createdAt)
        {
            var order = new Order(
                submitOrder.OrderId,
                submitOrder.UserId,
                submitOrder.CurrencyId,
                submitOrder.Price,
                submitOrder.Quantity,
                submitOrder.OrderType.BuildOrderType(),
                createdAt
                );

            return order;
        }
        public static Order BuildOrder(this Command.PlaceOrder placeOrder, DateTime createdAt)
        {
            var order = new Order(
                placeOrder.OrderId,
                placeOrder.UserId,
                placeOrder.CurrencyId,
                placeOrder.Price,
                placeOrder.Quantity,
                placeOrder.OrderType.BuildOrderType(),
                createdAt
                );

            return order;
        }
        public static OrderType BuildOrderType(this CommandType.OrderType orderType)
        {
            switch(orderType)
            {
                case CommandType.OrderType.BUY:
                    return OrderType.BUY;
                case CommandType.OrderType.SELL:
                    return OrderType.SELL;
                default:
                    throw new NotImplementedException(orderType + " not supported");
            }
        }
    }
}
