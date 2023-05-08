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
using MassTransit;
using CesarBmx.Ordering.Domain.Builders;
using CesarBmx.Shared.Messaging.Ordering.Events;
using CesarBmx.Ordering.Application.Requests;
using CesarBmx.Ordering.Domain.Models;
using MassTransit.Transports;

namespace CesarBmx.Ordering.Application.Services
{
    public class OrderService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        private readonly ActivitySource _activitySource;
        private readonly IBus _bus;

        public OrderService(
            MainDbContext mainDbContext,
            IMapper mapper,
            ILogger<OrderService> logger,
            ActivitySource activitySource,
            IBus bus)
        {
            _mainDbContext = mainDbContext;
            _mapper = mapper;
            _logger = logger;
            _activitySource = activitySource;
            _bus = bus;
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
        public async Task<Responses.Order> SubmitOrder(SubmitOrder submitOrder)
        {
            // Start watch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Start span
            using var span = _activitySource.StartActivity(nameof(PlaceOrder));

            // New order
            var order = OrderBuilder.BuildOrder(submitOrder, DateTime.UtcNow);

            // Add
            await _mainDbContext.Orders.AddAsync(order);           

            // Response
            var response = _mapper.Map<Responses.Order>(order);

            // Event
            var orderSubmitted = _mapper.Map<List<OrderSubmitted>>(order);

            // Publish event
            await _bus.Publish(orderSubmitted);

            // Save
            await _mainDbContext.SaveChangesAsync();

            // Stop watch
            stopwatch.Stop();

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", nameof(OrderSubmitted), Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);

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
            var order = OrderBuilder.BuildOrder(placeOrder, DateTime.UtcNow);

            // Add
            await _mainDbContext.Orders.AddAsync(order);

            // TODO: Place order on Binance

            // Mark as placed
            order.MarkAsPlaced();

            // Response
            var response = _mapper.Map<Responses.Order>(order);

            // Event
            var ordersPlaced = _mapper.Map<List<OrderPlaced>>(order);

            // Publish event
            await _bus.Publish(ordersPlaced);

            // Save
            await _mainDbContext.SaveChangesAsync();

            // Stop watch
            stopwatch.Stop();

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", nameof(OrderPlaced), Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);

            // Return
            return response;
        }
        public async Task<Responses.Order> FillOrder(FillOrder fillOrder)
        {
            // Start watch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Start span
            using var span = _activitySource.StartActivity(nameof(PlaceOrder));

            // Get order
            var order = await _mainDbContext.Orders.FirstOrDefaultAsync(x => x.OrderId == fillOrder.OrderId);

            // Mark as filled
            order.MarkAsFilled();

            // Update
            _mainDbContext.Orders.Update(order);

            // Response
            var response = _mapper.Map<Responses.Order>(order);

            // Event
            var orderFilled = _mapper.Map<List<OrderFilled>>(order);

            // Publish event
            await _bus.Publish(orderFilled);

            // Save
            await _mainDbContext.SaveChangesAsync();

            // Stop watch
            stopwatch.Stop();

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", nameof(OrderFilled), Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);

            // Return
            return response;
        }

        public async Task<Order> PlaceOrder(Order order)
        {
            // Start watch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Start span
            using var span = _activitySource.StartActivity(nameof(PlaceOrder));

            // TODO: Place it on Binance

            // Mark it as placed
            order.MarkAsPlaced();

            // Update order
            _mainDbContext.Orders.Update(order);

            // Save
            await _mainDbContext.SaveChangesAsync();

            // Event
            var ordersPlaced = _mapper.Map<List<OrderPlaced>>(order);

            // Publish event
            await _bus.Publish(ordersPlaced);

            // Stop watch
            stopwatch.Stop();

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", nameof(OrderPlaced), Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);

            // Return
            return order;
        }
    }
}
