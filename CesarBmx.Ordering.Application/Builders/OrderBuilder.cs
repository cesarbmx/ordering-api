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
    public static Order BuildOrder(this CesarBmx.Shared.Messaging.Ordering.Commands.PlaceOrder placeOrder, DateTime createdAt)
    {
        var order = new Order(
            placeOrder.OrderId,
            placeOrder.UserId,
            placeOrder.CurrencyId,
            placeOrder.Price,
            placeOrder.Quantity,
            BuildOrderType(placeOrder.OrderType),
            createdAt
            );

        return order;
    }
    public static OrderType BuildOrderType(this CesarBmx.Shared.Messaging.Ordering.Types.OrderType orderType)
    {
        switch(orderType)
        {
            case CesarBmx.Shared.Messaging.Ordering.Types.OrderType.BUY:
                return OrderType.BUY;
            case CesarBmx.Shared.Messaging.Ordering.Types.OrderType.SELL:
                return OrderType.SELL;
        }
        throw new NotImplementedException(nameof(orderType));
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
