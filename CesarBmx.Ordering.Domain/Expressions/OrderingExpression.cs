using System;
using System.Linq.Expressions;
using CesarBmx.Ordering.Domain.Models;

namespace CesarBmx.Ordering.Domain.Expressions
{
    public static class NotificationExpression
    {
        public static Expression<Func<Message, bool>> Filter(string userId = null)
        {
            return x => string.IsNullOrEmpty(userId) || x.UserId == userId;
        }
        public static Expression<Func<Message, bool>> PendingNotification()
        {
            return x => !x.SentTime.HasValue;
        }
    }
}
