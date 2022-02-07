using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain;
using CheckoutChallenge.Application.Domain.Events;

namespace CheckoutChallenge.Application.Bus
{
    public class InProcessBus : IEventPublisher, IEventSubscriber
    {
        private readonly Dictionary<Type, List<Func<object, Task>>> _routes = new Dictionary<Type, List<Func<object, Task>>>();

        public void Subscribe<T>(Func<T, Task> handler) where T : Event
        {
            if (!_routes.TryGetValue(typeof(T), out var handlers))
            {
                handlers = new List<Func<object, Task>>();
                _routes.Add(typeof(T), handlers);
            }

            handlers.Add((x => handler((T)x)));
        }


        public async Task PublishAsync<T>(T @event) where T : Event
        {
            if (!_routes.TryGetValue(@event.GetType(), out var handlers)) return;

            foreach (var handler in handlers)
            {
                await handler(@event);
            }
        }
    }
}