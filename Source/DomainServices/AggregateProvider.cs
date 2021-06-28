using System;
using System.Linq;
using Hive.SeedWorks.DomainModel;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.Monads;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Провайдер получения экземпляра агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public class AggregateProvider<TBoundedContext, TAggregate, TModel> :
        IAggregateProvider<TBoundedContext, TAggregate>
        where TBoundedContext : IBoundedContext
		where TAggregate : IAggregate<TBoundedContext>
        where TModel : IAnemicModel<TBoundedContext>
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

        private TAggregate DecorateModel(TModel model)
        => model
            .PipeTo(anemicModel = Aggregate<TBoundedContext>
                .CreateInstance(model));
    }
}
