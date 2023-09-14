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
                NotificationType = NotificationType.TELEGRAM,
                PhoneNumber = "+34666333222",
                Text = order.BuildNotificationText()
            };

            return sendNotification;
        }
        public static string BuildNotificationText(this Order order)
        {
            switch (order.OrderType)
            {
                case OrderType.BUY:
                    return "Order placed: " + order.Price + "order.CurrencyId bought";
                case OrderType.SELL:
                    return "Order placed: " + order.Price + "order.CurrencyId sold";
                default:
                    throw new NotImplementedException(order.OrderType + " not supported");
            }
        }
    }
}
