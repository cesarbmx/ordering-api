// using CesarBmx.Shared.Messaging.Ordering.Events;
//using MassTransit;
//using Microsoft.Extensions.Logging;
//using System.Diagnostics;
//using System;
//using System.Threading.Tasks;
//using AutoMapper;
//using CesarBmx.Ordering.Persistence.Contexts;
//using CesarBmx.Ordering.Application.Services;
//using Microsoft.EntityFrameworkCore;

//namespace CesarBmx.Ordering.Application.Consumers
//{
//    public class OrderPlacedConsumer : IConsumer<OrderPlaced>
//    {
//        private readonly MainDbContext _mainDbContext;
//        private readonly IMapper _mapper;
//        private readonly ILogger<OrderPlacedConsumer> _logger;
//        private readonly ActivitySource _activitySource;

//        public OrderPlacedConsumer(
//            MainDbContext mainDbContext,
//            IMapper mapper,
//            ILogger<OrderPlacedConsumer> logger,
//            ActivitySource activitySource)
//        {
//            _mainDbContext = mainDbContext;
//            _mapper = mapper;
//            _logger = logger;
//            _activitySource = activitySource;
//        }

//        public async Task Consume(ConsumeContext<OrderPlaced> context)
//        {
//            try
//            {
//                // Start watch
//                var stopwatch = new Stopwatch();
//                stopwatch.Start();

//                // Start span
//                using var span = _activitySource.StartActivity(nameof(OrderPlaced));

//                // Event
//                var orderSubmitted = context.Message;

//                // TODO: Place order on Binance

//                // Get order
//                var order = await _mainDbContext.Orders.FirstOrDefaultAsync(x => x.OrderId == orderSubmitted.OrderId);

//                // Mark as placed
//                order.MarkAsPlaced();

//                // Save
//                await _mainDbContext.SaveChangesAsync();                        

//                // Stop watch
//                stopwatch.Stop();

//                // Log
//                _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", nameof(OrderPlaced), Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);
//            }
//            catch(Exception ex)
//            {
//                // Log
//                _logger.LogError(ex, ex.Message);
//            }
//        }
//    }
//}
