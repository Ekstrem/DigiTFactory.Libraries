using System.Collections.Generic;
using Hive.SeedWorks.Events;

namespace Hive.SeedWorks.TacticalPatterns.Abstracts
{
    public abstract class AggregateResultDiff<TBoundedContext, TAggregate>
        where TAggregate : IAggregate<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly TAggregate _aggregate;
        private readonly CommandToAggregate _command;
        private readonly IDictionary<string, IValueObject> _changedValueObjects;

        protected AggregateResultDiff(
            TAggregate aggregate,
            CommandToAggregate command,
            IDictionary<string, IValueObject> changedValueObjects)
        {
            _aggregate = aggregate;
            _command = command;
            _changedValueObjects = changedValueObjects;
        }

        private TAggregate Aggregate => _aggregate;

        private CommandToAggregate Command => _command;

        private IDictionary<string, IValueObject> ChangedValueObjects => _changedValueObjects;
    }
}