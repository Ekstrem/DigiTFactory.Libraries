using Hive.SeedWorks.Result;

namespace Hive.SeedWorks.TacticalPatterns.Abstracts
{
    public interface IDomainCommandExecutor<TBoundedContext, TAggregate>
        where TAggregate : IAggregate<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        //string Name { get; }

        AggregateResultDiff<TBoundedContext, TAggregate> Handle(DomainCommand command);
    }
}
