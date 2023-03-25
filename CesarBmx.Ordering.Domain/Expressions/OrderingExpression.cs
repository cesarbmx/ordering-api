using System;
using System.Linq.Expressions;
using CesarBmx.Ordering.Domain.Models;

namespace CesarBmx.Ordering.Domain.Expressions
{
    public static class NotificationExpression
    {
        public static Expression<Func<Order, bool>> Filter(string userId = null)
        {
            return x => string.IsNullOrEmpty(userId) || x.UserId == userId;
        }
    }
}
