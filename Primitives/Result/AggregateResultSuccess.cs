using System.Collections.Generic;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.LifeCircle;

namespace Hive.SeedWorks.Result
{
    /// <summary>
    /// Результат выполнения бизнес-операции в агрегате.
    /// </summary>
    public abstract class AggregateResultSuccess<TBoundedContext, TAggregate, TKey> :
        AggregateResult<TBoundedContext, TAggregate, TKey>
        where TAggregate : IAggregate<TBoundedContext, TKey>
        where TBoundedContext : IBoundedContext
    {
        protected AggregateResultSuccess(
            TAggregate aggregate,
            CommandToAggregate command,
            IDictionary<string, IValueObject> changedValueObjects)
            : base(aggregate, command, changedValueObjects) { }
    }
}