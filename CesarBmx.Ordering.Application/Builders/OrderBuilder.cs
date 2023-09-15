using System;
using CesarBmx.Ordering.Application.Requests;
using CesarBmx.Ordering.Domain.Models;
using CesarBmx.Ordering.Domain.Types;
using CesarBmx.Shared.Messaging.Notification.Commands;
using CesarBmx.Shared.Common.Extensions;

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
    public static SendMessage BuildSendMessage(this Order order)
    {
        var sendNotification = new SendMessage
        {
            NotificationId = Guid.NewGuid(),
            UserId = order.UserId,
            Text = order.BuildMessageText(),
            ScheduledFor = null
        };

        return sendNotification;
    }
    public static string BuildMessageText(this Order order)
    {
        switch (order.OrderType)
        {
            case OrderType.BUY:
                return "Order placed: " + order.Price.Normalize() + " bought";
            case OrderType.SELL:
                return "Order placed: " + order.Price.Normalize() + " sold";
            default:
                throw new NotImplementedException(order.OrderType + " not supported");
        }
    }
}
