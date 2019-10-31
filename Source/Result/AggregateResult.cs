using System.Collections.Generic;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.SeedWorks.Result
{
    /// <summary>
    /// Результат выполнения бизнес-операции в агрегате.
    /// </summary>
    public abstract class AggregateResult<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly IDomainEvent<TBoundedContext> _domainEvent;
        private readonly IAggregate<TBoundedContext> _aggregate;

        protected AggregateResult(
            IAggregate<TBoundedContext> aggregate,
            CommandToAggregate command,
            IDictionary<string, IValueObject> changedValueObjects)
        {
            _aggregate = aggregate;
            _domainEvent = new DomainEvent<TBoundedContext>(
                aggregate.Id, aggregate.VersionNumber, command, changedValueObjects);
        }

        /// <summary>
        /// Событие предметной области.
        /// </summary>
        public IDomainEvent<TBoundedContext> Event => _domainEvent;

        /// <summary>
        /// Агрегат - источник события.
        /// </summary>
        public IAggregate<TBoundedContext> Aggregate => _aggregate;
    }
}