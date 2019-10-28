using System.Collections.Generic;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.LifeCircle;

namespace Hive.SeedWorks.Result
{
    /// <summary>
    /// Результат выполнения бизнес-операции в агрегате.
    /// </summary>
    public class AggregateResultFailure<TBoundedContext> :
        AggregateResult<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly string _exceptionReason;

        internal AggregateResultFailure(
            IAggregate<TBoundedContext> aggregate,
            CommandToAggregate command,
            IDictionary<string, IValueObject> changedValueObjects,
            string exceptionReason)
            : base(aggregate, command, changedValueObjects)
        {
            _exceptionReason = exceptionReason;
        }

        public string ExceptionReason => _exceptionReason;
    }
}