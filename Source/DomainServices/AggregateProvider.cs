using System;
using System.Linq;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.Monads;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns.Abstracts;
using Hive.SeedWorks.TacticalPatterns.Repository;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Провайдер получения экземпляра агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public class AggregateProvider<TBoundedContext, TAggregate> :
        IAggregateProvider<TBoundedContext, TAggregate>
        where TBoundedContext : IBoundedContext
		where TAggregate : IAggregate<TBoundedContext>
    {
        private readonly IRepository<TBoundedContext, TAggregate> _repository;
        private readonly IObserver<AggregateResultDiff<TBoundedContext, TAggregate>>[] _observers;

        public AggregateProvider(
            IRepository<TBoundedContext, TAggregate> repository,
            params IObserver<AggregateResultDiff<TBoundedContext, TAggregate>>[] observers)
        {
            _repository = repository;
            _observers = observers;
        }

        public TAggregate GetAggregate(Guid id, long version)
        {
            return (_repository.GetByIdAndVersion(id, version)
                    ?? DefaultAnemicModel<TBoundedContext>.Create(id))
                .PipeTo(DecorateModel);
        }

        public TAggregate GetAggregate(Guid correlationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class DefaultAnemicModel<TBoundedContext> : 
        IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private DefaultAnemicModel(Guid id)
        {
            
        }

        public ICommandMetadata PreviousOperation => null;

        public static DefaultAnemicModel<TBoundedContext> Create(Guid id)
            => new DefaultAnemicModel<TBoundedContext>(id);
    }
}
