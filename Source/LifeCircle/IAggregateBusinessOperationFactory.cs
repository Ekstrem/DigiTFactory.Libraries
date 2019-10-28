using System;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;

namespace Hive.SeedWorks.LifeCircle
{
    public interface IAggregateBusinessOperationFactory<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        Result<AggregateResultSuccess<TBoundedContext>, AggregateResultFailure<TBoundedContext>> Handle(
            IAnemicModel<TBoundedContext> input, CommandToAggregate command);
    }
}