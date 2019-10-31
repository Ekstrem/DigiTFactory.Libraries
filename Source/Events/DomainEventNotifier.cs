using Hive.SeedWorks.TacticalPatterns;

namespace Hive.SeedWorks.Events
{
    /// <summary>
    /// Нотификатор о доменных событиях.
    /// Базовый класс проксирования агрегата.
    /// </summary>
    /// <typeparam name="TBoundContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TAggregate">Тип агрегата.</typeparam>
    public abstract class DomainEventNotifier<TBoundContext, TAggregate>
        where TAggregate : IAggregate<TBoundContext>
        where TBoundContext : IBoundedContext
    {
        /// <summary>
        /// Агрегат, вызовы к которому необходимо проксировать.
        /// </summary>
        protected readonly TAggregate Aggregate;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="aggregate">Тип агрегата.</param>
        protected DomainEventNotifier(TAggregate aggregate)
            => Aggregate = aggregate;
    }
}