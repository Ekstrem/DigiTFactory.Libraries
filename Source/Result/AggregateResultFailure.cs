using System.Collections.Generic;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.LifeCircle;

namespace Hive.SeedWorks.Result
{
    /// <summary>
    /// Результат выполнения бизнес-операции в агрегате.
    /// </summary>
    public abstract class AggregateResultFailure<TBoundedContext, TAggregate, TKey> :
        AggregateResult<TBoundedContext, TAggregate, TKey>
        where TAggregate : IAggregate<TBoundedContext, TKey>
        where TBoundedContext : IBoundedContext
    {
        private readonly string _exceptionReason;

        protected AggregateResultFailure(
            TAggregate aggregate,
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