using System;
using CesarBmx.Ordering.Application.Requests;
using CesarBmx.Ordering.Domain.Models;
using CesarBmx.Ordering.Domain.Types;
using CesarBmx.Shared.Messaging.Notification.Commands;
using CesarBmx.Shared.Messaging.Notification.Types;
using Command = CesarBmx.Shared.Messaging.Ordering.Commands;
using CommandType = CesarBmx.Shared.Messaging.Ordering.Types;

namespace CesarBmx.Ordering.Domain.Builders
{
    public static class OrderBuilder
    {
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
        public static SendNotification BuildSendNotification(this Order order)
        {
            var sendNotification = new SendNotification
            {
                NotificationId = Guid.NewGuid(),
                UserId = order.UserId,
                Text = order.BuildNotificationText(),
                ScheduledFor = null
            };

            return sendNotification;
        }
        public static string BuildNotificationText(this Order order)
        {
            switch (order.OrderType)
            {
                case OrderType.BUY:
                    return "Order placed: " + order.Price + " bought";
                case OrderType.SELL:
                    return "Order placed: " + order.Price + " sold";
                default:
                    throw new NotImplementedException(order.OrderType + " not supported");
            }
        }
    }
}
