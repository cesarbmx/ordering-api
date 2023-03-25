using System;
using System.Collections.Generic;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.Ordering.Application.Responses;
using CesarBmx.Ordering.Domain.Types;

namespace CesarBmx.Ordering.Application.FakeResponses
{
    public static class FakeOrder
    {
        private static readonly DateTime _now = DateTime.UtcNow.StripSeconds();

        public static Order GetFake_Bitcoin()
        {
            return new Order
            {
                OrderId = Guid.NewGuid(),
                OrderType = OrderType.BUY,
                UserId = "master",
                CurrencyId = "BTC",
                OrderStatus = OrderStatus.PLACED,
                Price = 3200,
                Quantity = 100,
                PlacedAt = _now,
                FilledAt = null
            };
        }
        public static Order GetFake_EOS()
        {
            return new Order
            {
                OrderId = Guid.NewGuid(),
                OrderType = OrderType.BUY,
                UserId = "master",
                CurrencyId = "EOS",
                OrderStatus = OrderStatus.FILLED,
                Price = 3.5m,
                Quantity = 100,
                PlacedAt = _now,
                FilledAt = _now.AddMinutes(1)
            };
        }
        public static List<Order> GetFake_List()
        {
            return new List<Order>
            {
                GetFake_Bitcoin(),
                GetFake_EOS()
            };
        }
    }
}
