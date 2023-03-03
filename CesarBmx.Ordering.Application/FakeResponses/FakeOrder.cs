using System;
using System.Collections.Generic;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.Ordering.Application.Responses;

namespace CesarBmx.Ordering.Application.FakeResponses
{
    public static class FakeOrder
    {
        public static Order GetFake_Master()
        {
            return new Order
            {
                MessageId = Guid.NewGuid(),
                UserId = "master",
                Text = "Test message",
                PhoneNumber = "+34666555555",
                Time = DateTime.UtcNow.StripSeconds(),
                SentTime = null
            };
        }
        public static Order GetFake_CesarBmx()
        {
            return new Order
            {
                MessageId = Guid.NewGuid(),
                UserId = "cesarbmx",
                Text = "Test message",
                PhoneNumber = "+34666666666",
                Time = DateTime.UtcNow.StripSeconds(),
                SentTime = null
            };
        }
        public static List<Order> GetFake_List()
        {
            return new List<Order>
            {
                GetFake_Master(),
                GetFake_CesarBmx()
            };
        }
    }
}
