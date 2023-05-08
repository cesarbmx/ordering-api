using System;
using CesarBmx.Shared.Common.Extensions;
using MassTransit;
using CesarBmx.Shared.Messaging.Ordering.Events;
using CesarBmx.Shared.Messaging.Ordering.Commands;

namespace CesarBmx.Ordering.Application.Sagas
{
    public class OrderState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }

        public Guid OrderId { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime? PlacedAt { get; set; }
        public DateTime? FilledAt { get; set; }
        public DateTime? CancelledAt { get; set; }
    }

    public class OrderSagaStateMachine : MassTransitStateMachine<OrderState>
    {
        public OrderSagaStateMachine()
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
                    .PublishOrderSubmitted()
                    //.Schedule(ExpirationSchedule, context => context.Init<OrderExpired>(new OrderExpired { OrderId = context.Message.OrderId, ExpiredAt = DateTime.UtcNow.StripSeconds() }))
                    .TransitionTo(Submitted),
                 When(OrderPlaced)
                    .SetPlacingDetails()
                    .PublishOrderPlaced()
                    .TransitionTo(Placed));

            During(Submitted,
                When(OrderPlaced)
                    .SetPlacingDetails()
                    .PublishOrderPlaced()
                    .TransitionTo(Placed));

            During(Placed,
                When(OrderFilled)
                    .SetFillingDetails()
                    .PublishOrderFilled()
                    //.Unschedule(ExpirationSchedule)
                    .TransitionTo(Filled)
                    .Finalize());

            During(Placed,
                When(OrderCancelled)
                    .SetCancelationDetails()
                    .PublishOrderCancelled()
                    //.Unschedule(ExpirationSchedule)
                    .TransitionTo(Cancelled)
                    .Finalize());

            During(Placed,
               When(OrderExpired)
                   .SetExpirationDetails()
                   .PublishOrderCancelled()
                   //.Unschedule(ExpirationSchedule)
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

        public Schedule<OrderState, OrderExpired> ExpirationSchedule { get; }
    }

    public static class OrderStateMachineExtensions
    {
        public static EventActivityBinder<OrderState, OrderSubmitted> SetSubmissionDetails(
            this EventActivityBinder<OrderState, OrderSubmitted> binder)
        {
            return binder.Then(x =>
            {
                x.Saga.OrderId = x.Message.OrderId;
                x.Saga.SubmittedAt = x.Message.SubmittedAt;
            });
        }
        public static EventActivityBinder<OrderState, OrderPlaced> SetPlacingDetails(
           this EventActivityBinder<OrderState, OrderPlaced> binder)
        {
            return binder.Then(x =>
            {
                x.Saga.PlacedAt = x.Message.PlacedAt;
            });
        }
        public static EventActivityBinder<OrderState, OrderFilled> SetFillingDetails(
          this EventActivityBinder<OrderState, OrderFilled> binder)
        {
            return binder.Then(x =>
            {
                x.Saga.FilledAt = x.Message.FilledAt;
            });
        }
        public static EventActivityBinder<OrderState, OrderCancelled> SetCancelationDetails(
          this EventActivityBinder<OrderState, OrderCancelled> binder)
        {
            return binder.Then(x =>
            {
                x.Saga.CancelledAt = x.Message.CancelledAt;
            });
        }
        public static EventActivityBinder<OrderState, OrderExpired> SetExpirationDetails(
          this EventActivityBinder<OrderState, OrderExpired> binder)
        {
            return binder.Then(x =>
            {
                x.Saga.CancelledAt = x.Message.ExpiredAt;
            });
        }

        public static EventActivityBinder<OrderState, OrderSubmitted> PublishOrderSubmitted(
           this EventActivityBinder<OrderState, OrderSubmitted> binder)
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
        public static EventActivityBinder<OrderState, OrderPlaced> PublishOrderPlaced(
           this EventActivityBinder<OrderState, OrderPlaced> binder)
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
        public static EventActivityBinder<OrderState, OrderFilled> PublishOrderFilled(
           this EventActivityBinder<OrderState, OrderFilled> binder)
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
        public static EventActivityBinder<OrderState, OrderCancelled> PublishOrderCancelled(
          this EventActivityBinder<OrderState, OrderCancelled> binder)
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
        public static EventActivityBinder<OrderState, OrderExpired> PublishOrderCancelled(
          this EventActivityBinder<OrderState, OrderExpired> binder)
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
