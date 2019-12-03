using System;
using System.Collections.Generic;
using System.Linq;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.Monads;
using Hive.SeedWorks.TacticalPatterns.Repository;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Провайдер получения экземпляра агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public class AggregateProvider<TBoundedContext> :
        IAggregateProvider<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly IUnitOfWork<TBoundedContext> _unitOfWork;
        private readonly IBoundedContextScope<TBoundedContext> _scope;
        private readonly IList<IAggregateBusinessOperationFactory<TBoundedContext>> _operations;
        private readonly IList<IBusinessValidator<TBoundedContext>> _validators;

        public AggregateProvider(
            IUnitOfWork<TBoundedContext> unitOfWork,
            IBoundedContextScope<TBoundedContext> scope)
        {
            _unitOfWork = unitOfWork;
            _scope = scope;
        }

        public IAggregate<TBoundedContext> GetAggregateByIdAndVersion(Guid id, CommandToAggregate command)
            => _unitOfWork.QueryRepository.GetQueryable()
                .Where(f => f.Id == id)
                .Max(f => f.VersionNumber)
                .PipeTo(version => GetAggregateByIdAndVersion(id, version, command));

        public IAggregate<TBoundedContext> GetAggregateByIdAndVersion(Guid id, int version, CommandToAggregate command)
        {
            var anemicModel = _unitOfWork.QueryRepository.GetQueryable()
                .Single(f => f.Id == id && f.VersionNumber == version);
            var aggregate = Aggregate<TBoundedContext>
                .CreateInstance(anemicModel, _scope);
            return aggregate;
        }

        public IAggregate<TBoundedContext> NewAggregate(IAnemicModel<TBoundedContext> anemicModel, CommandToAggregate command)
            => Aggregate<TBoundedContext>.CreateInstance(
                anemicModel, _scope);

        public void SaveChanges(IAggregate<TBoundedContext> aggregate)
            => _unitOfWork.Save();
    }
}
