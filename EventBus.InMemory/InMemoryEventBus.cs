using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Events;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Microsoft.Extensions.Logging;

namespace DigiTFactory.Libraries.EventBus.InMemory
{
    /// <summary>
    /// In-Memory реализация IEventBus.
    /// Синхронный dispatch всем подписанным хендлерам в текущем процессе.
    /// </summary>
    public sealed class InMemoryEventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, ImmutableList<object>> _handlers = new();
        private readonly ILogger<InMemoryEventBus> _logger;

        public InMemoryEventBus(ILogger<InMemoryEventBus> logger)
        {
            _logger = logger;
        }

        public void Publish<TBoundedContext>(IDomainEvent<TBoundedContext> domainEvent)
            where TBoundedContext : IBoundedContext
        {
            if (!_handlers.TryGetValue(typeof(TBoundedContext), out var handlers))
                return;

            foreach (var handler in handlers)
            {
                try
                {
                    if (handler is IObserver<IDomainEvent<TBoundedContext>> observer)
                    {
                        observer.OnNext(domainEvent);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error dispatching domain event to handler {HandlerType} for {BoundedContext}",
                        handler.GetType().Name, typeof(TBoundedContext).Name);
                }
            }
        }

        public Task PublishAsync<TBoundedContext>(IDomainEvent<TBoundedContext> domainEvent)
            where TBoundedContext : IBoundedContext
        {
            Publish(domainEvent);
            return Task.CompletedTask;
        }

        public void Subscribe<TBoundedContext>(IDomainEventHandler<TBoundedContext> handler)
            where TBoundedContext : IBoundedContext
        {
            _handlers.AddOrUpdate(
                typeof(TBoundedContext),
                _ => ImmutableList.Create<object>(handler),
                (_, list) => list.Add(handler));

            _logger.LogDebug(
                "Subscribed handler {HandlerType} for {BoundedContext}",
                handler.GetType().Name, typeof(TBoundedContext).Name);
        }

        public void Unsubscribe<TBoundedContext>(IDomainEventHandler<TBoundedContext> handler)
            where TBoundedContext : IBoundedContext
        {
            _handlers.AddOrUpdate(
                typeof(TBoundedContext),
                _ => ImmutableList<object>.Empty,
                (_, list) => list.Remove(handler));

            _logger.LogDebug(
                "Unsubscribed handler {HandlerType} for {BoundedContext}",
                handler.GetType().Name, typeof(TBoundedContext).Name);
        }
    }
}
