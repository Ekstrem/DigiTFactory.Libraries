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
        private readonly IList<IAggregateBusinessOperationFactory<TBoundedContext>> _operations;
        private readonly IList<IBusinessValidator<TBoundedContext>> _validators;

        public AggregateProvider(
            IUnitOfWork<TBoundedContext> unitOfWork,
            IList<IAggregateBusinessOperationFactory<TBoundedContext>> operations,
            IList<IBusinessValidator<TBoundedContext>> validators)
        {
            _unitOfWork = unitOfWork;
            _operations = operations;
            _validators = validators;
        }

        public IAggregate<TBoundedContext> GetAggregateByIdAndVersion(Guid id, CommandToAggregate command) 
            => _unitOfWork.QueryRepository.GetQueryable()
                .Where(f => f.Root.Id == id)
                .Max(f => f.Root.VersionNumber)
                .PipeTo(version => GetAggregateByIdAndVersion(id, version, command));

        public IAggregate<TBoundedContext> GetAggregateByIdAndVersion(Guid id, int version, CommandToAggregate command)
        {
            var anemicModel = _unitOfWork.QueryRepository.GetQueryable()
                .Single(f => f.Root.Id == id && f.Root.VersionNumber == version);
            var aggregate = Aggregate<TBoundedContext>
                .CreateInstance(ComplexKey.Create(id, version, command), anemicModel, _operations, _validators);
            return aggregate;
        }

        public IAggregate<TBoundedContext> NewAggregate(IAnemicModel<TBoundedContext> anemicModel, CommandToAggregate command)
            => Aggregate<TBoundedContext>.CreateInstance(
                command.CreateNewVersion(),
                anemicModel, default, default);

        public void SaveChanges(IAggregate<TBoundedContext> aggregate) 
            => _unitOfWork.Save();
    }
}