using System.Collections.Generic;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.SeedWorks.Result
{
    /// <summary>
    /// Результат выполнения бизнес-операции в агрегате.
    /// </summary>
    public class AggregateResultSuccess<TBoundedContext> :
        AggregateResult<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        internal AggregateResultSuccess(
            IAggregate<TBoundedContext> aggregate,
            CommandToAggregate command,
            IDictionary<string, IValueObject> changedValueObjects)
            : base(aggregate, command, changedValueObjects) { }
    }
}