using System;
using Hive.SeedWorks.Result;

namespace Hive.SeedWorks.LifeCircle
{
    public interface IAggregateBusinessOperationFactory<in TIn, TBoundedContext, TAggregate, TKey>
        where TBoundedContext : IBoundedContext
        where TAggregate : IAggregate<TBoundedContext, TKey>
        where TIn : IAnemicModel<TBoundedContext>
    {
        Result<AggregateResultSuccess<TBoundedContext, TAggregate, TKey>,
            AggregateResultFailure<TBoundedContext, TAggregate, TKey>> Handle(TIn input);
    }


    public interface IAggregateBusinessOperationFactory<in TIn, TBoundedContext, TAggregate> :
        IAggregateBusinessOperationFactory<TIn, TBoundedContext, TAggregate, Guid>
        where TBoundedContext : IBoundedContext
        where TAggregate : IAggregate<TBoundedContext, Guid>
        where TIn : IAnemicModel<TBoundedContext>
    {
    }

    public interface IAggregateBusinessOperationFactory<in TIn, TBoundedContext> :
        IAggregateBusinessOperationFactory<TIn, TBoundedContext, IAggregate<TBoundedContext, Guid>, Guid>
        where TBoundedContext : IBoundedContext
        where TIn : IAnemicModel<TBoundedContext>
    {
    }

    public interface IAggregateBusinessOperationFactory<TBoundedContext> :
        IAggregateBusinessOperationFactory<IAnemicModel<TBoundedContext>, TBoundedContext, IAggregate<TBoundedContext, Guid>, Guid>
        where TBoundedContext : IBoundedContext
    {
    }
}