using System;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.Ordering.Domain.Builders;
using CesarBmx.Ordering.Domain.Types;


namespace CesarBmx.Ordering.Domain.Models
{
    public class Message 
    {
        public Guid MessageId { get; private set; }
        public string UserId { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Text { get; private set; }
        public DateTime? SentTime { get; private set; }
        public DateTime Time { get; private set; }
        public NotificationStatus NotificationStatus => NotificationBuilder.BuildNotificationStatus(SentTime);

        public Message() { }
        public Message(string userId, string phoneNumber, string text, DateTime time)
        {
            MessageId = Guid.NewGuid();
            UserId = userId;
            PhoneNumber = phoneNumber;
            Text = text;
            SentTime = null;
            Time = time;
        }

        public void MarkAsSent()
        {
            SentTime = DateTime.UtcNow.StripSeconds();
        }
    }
}
