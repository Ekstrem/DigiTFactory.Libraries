using System.Collections.Generic;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.LifeCircle;

namespace Hive.SeedWorks.Result
{
    /// <summary>
    /// Результат выполнения бизнес-операции в агрегате.
    /// </summary>
    public abstract class AggregateResult<TBoundedContext, TAggregate, TKey>
        where TAggregate : IAggregate<TBoundedContext, TKey>
        where TBoundedContext : IBoundedContext
    {
        private readonly IDomainEvent<TBoundedContext, TKey> _domainEvent;
        private readonly TAggregate _aggregate;

        protected AggregateResult(
            TAggregate aggregate,
            CommandToAggregate command,
            IDictionary<string, IValueObject> changedValueObjects)
        {
            _aggregate = aggregate;
            _domainEvent = new DomainEvent<TBoundedContext, TKey>(
                aggregate.Id, command, changedValueObjects);
        }

        /// <summary>
        /// Событие предметной области.
        /// </summary>
        public IDomainEvent<TBoundedContext, TKey> Event => _domainEvent;

        /// <summary>
        /// Агрегат - источник события.
        /// </summary>
        public TAggregate Aggregate => _aggregate;
    }
}