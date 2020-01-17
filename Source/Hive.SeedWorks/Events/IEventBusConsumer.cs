using Hive.SeedWorks.TacticalPatterns;

namespace Hive.SeedWorks.Events
{
    /// <summary>
    /// Потребитель шины событий.
    /// </summary>
    public interface IEventBusConsumer
    {
        /// <summary>
        /// Подписаться на доменное событие.
        /// </summary>
        /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
        void Subscribe<TBoundedContext>(IDomainEventHandler<TBoundedContext> handler)
            where TBoundedContext : IBoundedContext;

        /// <summary>
        /// Отписаться от доменного события.
        /// </summary>
        /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
        void Unsubscribe<TBoundedContext>(IDomainEventHandler<TBoundedContext> handler)
            where TBoundedContext : IBoundedContext;
    }
}