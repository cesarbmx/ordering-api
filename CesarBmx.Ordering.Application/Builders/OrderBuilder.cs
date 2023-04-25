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
        public static Order BuildOrder(this SubmitOrder placeOrder, DateTime createdAt)
        {
            var order = new Order(
                placeOrder.UserId,
                placeOrder.CurrencyId,
                placeOrder.Price,
                placeOrder.Quantity,
                placeOrder.OrderType,
                createdAt
                );

            return order;
        }
        public static Order BuildOrder(this Command.SubmitOrder placeOrder, DateTime createdAt)
        {
            var order = new Order(
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
