using System.Collections.Generic;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks.Result
{
    /// <summary>
    /// Результат выполнения бизнес-операции в агрегате.
    /// </summary>
    public sealed class AggregateResult<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly IDomainEvent<TBoundedContext> _domainEvent;
        private readonly IAggregate<TBoundedContext> _aggregate;

        internal AggregateResult(
            IAggregate<TBoundedContext> aggregate,
            CommandToAggregate command,
            IDictionary<string, IValueObject> changedValueObjects)
        {
            _aggregate = aggregate;
            _domainEvent = new DomainEvent<TBoundedContext>(
                aggregate.Id, command, changedValueObjects);
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
