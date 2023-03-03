using System;
using CesarBmx.Notification.Domain.Types;

namespace CesarBmx.Notification.Application.Responses
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Text { get; set; }
        public DateTime? SentTime { get; set; }
        public DateTime Time { get; set; }
        public NotificationStatus NotificationStatus  { get; set; }
    }
}
