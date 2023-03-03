using System;
using CesarBmx.Ordering.Domain.Types;

namespace CesarBmx.Ordering.Application.Responses
{
    public class Order
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
