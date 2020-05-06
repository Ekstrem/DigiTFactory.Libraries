using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.Monads;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks.TacticalPatterns
{
    public class DomainCommandExecutor<TBoundedContext, TAggregate>
        : IDomainCommandExecutor<TBoundedContext, TAggregate>
        where TAggregate : IAggregate<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly IAggregateProvider<TBoundedContext, TAggregate> _provider;

        protected DomainCommandExecutor(
            IAggregateProvider<TBoundedContext, TAggregate> provider)
        {
            _provider = provider;
        }

        public AggregateResultDiff<TBoundedContext, TAggregate> Handle(DomainCommand command)
        {
            var aggregate = _provider.GetAggregate(command.Key.Id, command.Key.Version);
            
            //var dto = AnemicModelBase.Create(command.Metadata)
            
            throw new System.NotImplementedException();
        }
    }
}