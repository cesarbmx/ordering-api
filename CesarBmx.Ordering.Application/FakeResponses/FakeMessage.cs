using System;
using System.Collections.Generic;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.Ordering.Application.Responses;

namespace CesarBmx.Ordering.Application.FakeResponses
{
    public static class FakeMessage
    {
        public static Message GetFake_Master()
        {
            return new Message
            {
                MessageId = Guid.NewGuid(),
                UserId = "master",
                Text = "Test message",
                PhoneNumber = "+34666555555",
                Time = DateTime.UtcNow.StripSeconds(),
                SentTime = null
            };
        }
        public static Message GetFake_CesarBmx()
        {
            return new Message
            {
                MessageId = Guid.NewGuid(),
                UserId = "cesarbmx",
                Text = "Test message",
                PhoneNumber = "+34666666666",
                Time = DateTime.UtcNow.StripSeconds(),
                SentTime = null
            };
        }
        public static List<Message> GetFake_List()
        {
            return new List<Message>
            {
                GetFake_Master(),
                GetFake_CesarBmx()
            };
        }
    }
}
