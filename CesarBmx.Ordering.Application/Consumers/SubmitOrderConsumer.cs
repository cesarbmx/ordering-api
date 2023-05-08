using CesarBmx.Ordering.Domain.Builders;
using CesarBmx.Shared.Messaging.Ordering.Commands;
using CesarBmx.Shared.Messaging.Ordering.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using AutoMapper;
using CesarBmx.Ordering.Persistence.Contexts;

namespace CesarBmx.Ordering.Application.Consumers
{
    public class SubmitOrderConsumer : IConsumer<SubmitOrder>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<SubmitOrderConsumer> _logger;
        private readonly ActivitySource _activitySource;

        public SubmitOrderConsumer(
            MainDbContext mainDbContext,
            IMapper mapper,
            ILogger<SubmitOrderConsumer> logger,
            ActivitySource activitySource)
        {
            _mainDbContext = mainDbContext;
            _mapper = mapper;
            _logger = logger;
            _activitySource = activitySource;
        }

        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            try
            {
                // Start watch
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Start span
                using var span = _activitySource.StartActivity(nameof(SubmitOrder));

                // Command
                var submitOrder = context.Message;

                // New order
                var order = OrderBuilder.BuildOrder(submitOrder, DateTime.UtcNow);

                // Add
                await _mainDbContext.Orders.AddAsync(order);

                // Save
                await _mainDbContext.SaveChangesAsync();                

                // Response
                await context.RespondAsync(order);

                // Stop watch
                stopwatch.Stop();

                // Log
                _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", nameof(OrderSubmitted), Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);
            }
            catch(Exception ex)
            {
                // Log
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
