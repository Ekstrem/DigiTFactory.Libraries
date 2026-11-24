using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Threading.Tasks;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Events;
using Microsoft.Extensions.Logging;

namespace DigiTFactory.Libraries.EventBus.InMemory
{
    /// <summary>
    /// In-Memory реализация IEventBus.
    /// PublishAsync сначала ожидает durable-подписчиков (<see cref="IAsyncDomainEventHandler{TBoundedContext}"/>),
    /// затем диспатчит асинхронные IObserver-проекции. Ошибка durable-записи пробрасывается вызывающему
    /// и останавливает проекции; ошибка async-проекции логируется и не блокирует команду.
    /// </summary>
    public sealed class InMemoryEventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, ImmutableList<object>> _handlers = new();
        private readonly ILogger<InMemoryEventBus> _logger;

        public InMemoryEventBus(ILogger<InMemoryEventBus> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Синхронная совместимость для IObserver-подписчиков. Для durable-записи используйте PublishAsync:
        /// синхронный Publish не ожидает IAsyncDomainEventHandler.
        /// </summary>
        public void Publish<TBoundedContext>(IDomainEvent<TBoundedContext> domainEvent)
            where TBoundedContext : IBoundedContext
        {
            if (!_handlers.TryGetValue(typeof(TBoundedContext), out var handlers))
                return;

            foreach (var handler in handlers)
            {
                DispatchObserver(handler, domainEvent);
            }
        }

        /// <summary>
        /// Публикует событие и завершается только после durable-записи. Async-проекции запускаются после
        /// успешного сохранения и не ожидаются. Если durable-подписчик также реализует IObserver,
        /// событие доставляется ему только один раз — через HandleAsync.
        /// </summary>
        public async Task PublishAsync<TBoundedContext>(IDomainEvent<TBoundedContext> domainEvent)
            where TBoundedContext : IBoundedContext
        {
            if (!_handlers.TryGetValue(typeof(TBoundedContext), out var handlers))
                return;

            foreach (var handler in handlers)
            {
                if (handler is IAsyncDomainEventHandler<TBoundedContext> durableHandler)
                {
                    await durableHandler.HandleAsync(domainEvent).ConfigureAwait(false);
                }
            }

            foreach (var handler in handlers)
            {
                if (handler is IAsyncDomainEventHandler<TBoundedContext>)
                    continue;

                DispatchObserver(handler, domainEvent);
            }
        }

        private void DispatchObserver<TBoundedContext>(object handler, IDomainEvent<TBoundedContext> domainEvent)
            where TBoundedContext : IBoundedContext
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
                    "Error dispatching domain event to async projection {HandlerType} for {BoundedContext}",
                    handler.GetType().Name, typeof(TBoundedContext).Name);
            }
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
