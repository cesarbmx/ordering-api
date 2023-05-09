using System;
using CesarBmx.Shared.Common.Extensions;
using MassTransit;
using CesarBmx.Shared.Messaging.Ordering.Events;
using CesarBmx.Shared.Messaging.Ordering.Commands;

namespace CesarBmx.Ordering.Application.Sagas
{
    public class OrderSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }

        public Guid OrderId { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime? PlacedAt { get; set; }
        public DateTime? FilledAt { get; set; }
        public DateTime? CancelledAt { get; set; }
    }

    public class OrderSaga : MassTransitStateMachine<OrderSagaState>
    {
        public OrderSaga()
        {
            InstanceState(x => x.CurrentState, Submitted, Placed, Filled, Cancelled, Expired);

            Event(() => OrderSubmitted, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => OrderPlaced, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => OrderFilled, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => OrderCancelled, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => OrderExpired, x => x.CorrelateById(m => m.Message.OrderId));

            //Schedule(() => ExpirationSchedule, x => x.OrderId, x => x.Delay = TimeSpan.FromHours(1));

            Initially(
                When(OrderSubmitted)
                    .SetSubmissionDetails()
                    //.PublishOrderSubmitted()
                    //.Schedule(ExpirationSchedule, context => context.Init<OrderExpired>(new OrderExpired { OrderId = context.Message.OrderId, ExpiredAt = DateTime.UtcNow.StripSeconds() }))
                    .TransitionTo(Submitted));               

            During(Submitted,
                When(OrderPlaced)
                    .SetPlacingDetails()
                    //.Unschedule(ExpirationSchedule)
                    .TransitionTo(Placed)
                    .Finalize());

            During(Placed,
                When(OrderFilled)
                    .SetFillingDetails()
                    //.Unschedule(ExpirationSchedule)
                    .TransitionTo(Filled)
                    .Finalize());

            During(Placed,
                When(OrderCancelled)
                    .SetCancelationDetails()
                    //.Unschedule(ExpirationSchedule)
                    .TransitionTo(Cancelled)
                    .Finalize());

            During(Placed,
               When(OrderExpired)
                   .SetExpirationDetails()
                   //.PublishOrderCancelled()
                   .Unschedule(ExpirationSchedule)
                   .TransitionTo(Cancelled)
                   .Finalize());
        }

        public Event<OrderSubmitted> OrderSubmitted { get; private set; }
        public Event<OrderPlaced> OrderPlaced { get; private set; }
        public Event<OrderFilled> OrderFilled { get; private set; }
        public Event<OrderCancelled> OrderCancelled { get; private set; }
        public Event<OrderExpired> OrderExpired { get; private set; }

        public State Submitted { get; private set; }
        public State Placed { get; private set; }
        public State Filled { get; private set; }
        public State Cancelled { get; private set; }
        public State Expired { get; private set; }

        public Schedule<OrderSagaState, OrderExpired> ExpirationSchedule { get; }
    }

    public static class OrderSagaExtensions
    {
        public static EventActivityBinder<OrderSagaState, OrderSubmitted> SetSubmissionDetails(
            this EventActivityBinder<OrderSagaState, OrderSubmitted> binder)
        {
            return binder.Then(x =>
            {
                x.Saga.OrderId = x.Message.OrderId;
                x.Saga.SubmittedAt = x.Message.SubmittedAt;
            });
        }
        public static EventActivityBinder<OrderSagaState, OrderPlaced> SetPlacingDetails(
           this EventActivityBinder<OrderSagaState, OrderPlaced> binder)
        {
            return binder.Then(x =>
            {
                x.Saga.PlacedAt = x.Message.PlacedAt;
            });
        }
        public static EventActivityBinder<OrderSagaState, OrderFilled> SetFillingDetails(
          this EventActivityBinder<OrderSagaState, OrderFilled> binder)
        {
            return binder.Then(x =>
            {
                x.Saga.FilledAt = x.Message.FilledAt;
            });
        }
        public static EventActivityBinder<OrderSagaState, OrderCancelled> SetCancelationDetails(
          this EventActivityBinder<OrderSagaState, OrderCancelled> binder)
        {
            return binder.Then(x =>
            {
                x.Saga.CancelledAt = x.Message.CancelledAt;
            });
        }
        public static EventActivityBinder<OrderSagaState, OrderExpired> SetExpirationDetails(
          this EventActivityBinder<OrderSagaState, OrderExpired> binder)
        {
            return binder.Then(x =>
            {
                x.Saga.CancelledAt = x.Message.ExpiredAt;
            });
        }

        public static EventActivityBinder<OrderSagaState, OrderSubmitted> PublishOrderSubmitted(
           this EventActivityBinder<OrderSagaState, OrderSubmitted> binder)
        {
            return binder.PublishAsync(context => context.Init<OrderSubmitted>(new OrderSubmitted
            {

                // TODO: Automapper

                OrderId = context.Message.OrderId,
                UserId = context.Message.UserId,
                CurrencyId = context.Message.CurrencyId,
                Price = context.Message.Price,
                OrderType = context.Message.OrderType,
                Quantity = context.Message.Quantity,
                SubmittedAt = context.Message.SubmittedAt
            }));
        }
        public static EventActivityBinder<OrderSagaState, OrderPlaced> PublishOrderPlaced(
           this EventActivityBinder<OrderSagaState, OrderPlaced> binder)
        {
            return binder.PublishAsync(context => context.Init<OrderPlaced>(new OrderPlaced
            {

                // TODO: Automapper

                OrderId = context.Message.OrderId,
                UserId = context.Message.UserId,
                CurrencyId = context.Message.CurrencyId,
                Price = context.Message.Price,
                OrderType = context.Message.OrderType,
                Quantity = context.Message.Quantity,
                PlacedAt = context.Message.PlacedAt
            }));
        }
        public static EventActivityBinder<OrderSagaState, OrderFilled> PublishOrderFilled(
           this EventActivityBinder<OrderSagaState, OrderFilled> binder)
        {
            return binder.PublishAsync(context => context.Init<OrderFilled>(new OrderFilled
            {

                // TODO: Automapper

                OrderId = context.Message.OrderId,
                UserId = context.Message.UserId,
                CurrencyId = context.Message.CurrencyId,
                Price = context.Message.Price,
                OrderType = context.Message.OrderType,
                Quantity = context.Message.Quantity,
                FilledAt = context.Message.FilledAt
            }));
        }
        public static EventActivityBinder<OrderSagaState, OrderCancelled> PublishOrderCancelled(
          this EventActivityBinder<OrderSagaState, OrderCancelled> binder)
        {
            return binder.PublishAsync(context => context.Init<OrderCancelled>(new OrderCancelled
            {

                // TODO: Automapper

                OrderId = context.Message.OrderId,
                UserId = context.Message.UserId,
                CurrencyId = context.Message.CurrencyId,
                Price = context.Message.Price,
                OrderType = context.Message.OrderType,
                Quantity = context.Message.Quantity,
                CancelledAt = context.Message.CancelledAt
            }));
        }
        public static EventActivityBinder<OrderSagaState, OrderExpired> PublishOrderCancelled(
          this EventActivityBinder<OrderSagaState, OrderExpired> binder)
        {
            return binder.PublishAsync(context => context.Init<OrderExpired>(new OrderExpired
            {

                // TODO: Automapper

                OrderId = context.Message.OrderId,
                UserId = context.Message.UserId,
                CurrencyId = context.Message.CurrencyId,
                Price = context.Message.Price,
                OrderType = context.Message.OrderType,
                Quantity = context.Message.Quantity,
                ExpiredAt = context.Message.ExpiredAt
            }));
        }
    }
}