using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CesarBmx.Shared.Application.Exceptions;
using CesarBmx.Ordering.Application.Messages;
using CesarBmx.Ordering.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CesarBmx.Shared.Messaging.CryptoWatcher.Events;
using MassTransit;
using CesarBmx.Ordering.Application.Requests;
using CesarBmx.Ordering.Domain.Builders;
using CesarBmx.Shared.Messaging.Ordering.Events;

namespace CesarBmx.Ordering.Application.Services
{
    public class OrderService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        private readonly ActivitySource _activitySource;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderService(
            MainDbContext mainDbContext,
            IMapper mapper,
            ILogger<OrderService> logger,
            ActivitySource activitySource,
            IPublishEndpoint publishEndpoint)
        {
            _mainDbContext = mainDbContext;
            _mapper = mapper;
            _logger = logger;
            _activitySource = activitySource;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<List<Responses.Order>> GetOrders(string userId)
        {
            // Get all orders
            var orders = await _mainDbContext.Orders.Where(x => x.UserId == userId).ToListAsync();

            // Response
            var response = _mapper.Map<List<Responses.Order>>(orders);

            // Return
            return response;
        }
        public async Task<Responses.Order> GetOrder(Guid orderId)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetOrder));

            // Get order
            var order = await _mainDbContext.Orders.FindAsync(orderId);

            // Order not found
            if (order == null) throw new NotFoundException(OrderMessage.OrderNotFound);

            // Response
            var response = _mapper.Map<Responses.Order>(order);

            // Return
            return response;
        }

        public async Task<Responses.Order> PlaceOrder(PlaceOrder placeOrder)
        {
            // Start watch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Start span
            using var span = _activitySource.StartActivity(nameof(PlaceOrder));

            // New order
            var newOrder = OrderFactory.CreateOrder(placeOrder, DateTime.UtcNow);

            // Add
            await _mainDbContext.Orders.AddAsync(newOrder);

            // Save
            await _mainDbContext.SaveChangesAsync();

            // Response
            var response = _mapper.Map<Responses.Order>(newOrder);

            // Event
            var orderTriggered = _mapper.Map<List<OrderTriggered>>(newOrder);

            // Publish event
            await _publishEndpoint.Publish(orderTriggered);

            // Stop watch
            stopwatch.Stop();

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", nameof(OrderPlaced), Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);

            // Return
            return response;
        }
    }
}
