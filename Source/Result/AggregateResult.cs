using System.Collections.Generic;
using System.Linq;
using Hive.SeedWorks.Definition;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.Invariants;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.SeedWorks.Result
{
    /// <summary>
    /// Результат выполнения бизнес-операции в агрегате.
    /// </summary>
    public abstract class AggregateResult<TBoundedContext, TModel>
        where TModel : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly BusinessOperationData<TBoundedContext, TModel> _businessOperationData;
        private readonly IBoundedContextDescription _boundedContext;

        protected AggregateResult(BusinessOperationData<TBoundedContext, TModel> businessOperationData)
        {
            _businessOperationData = businessOperationData;
            _boundedContext = BoundedContext<TBoundedContext>.GetInfo();
        }

        /// <summary>
        /// Событие предметной области.
        /// </summary>
        public IDomainEvent<TBoundedContext> Event => new DomainEvent<TBoundedContext>(
            _businessOperationData.Aggregate.Id,
            _businessOperationData.Aggregate.Version,
            CommandToAggregate.Commit(
                _businessOperationData.Model.CorrelationToken,
                _businessOperationData.Model.CommandName,
                _businessOperationData.Model.SubjectName,
                _businessOperationData.Model.Version),
            ChangeValueObjects,
            _boundedContext,
            Result,
            Reason.First());

        /// <summary>
        /// Данные бизнес операции.
        /// </summary>
        public BusinessOperationData<TBoundedContext, TModel> BusinessOperationData => _businessOperationData;

        /// <summary>
        /// Изменившиеся объект значения.
        /// </summary>
        public virtual IDictionary<string, IValueObject> ChangeValueObjects => _businessOperationData.GetValueObjects();
        
        /// <summary>
        /// Результат выполнения операции.
        /// </summary>
        public abstract DomainOperationResultEnum Result {get;}
        
        /// <summary>
        /// Причина в случае неуспеха выполнения операции.
        /// </summary>
        public abstract IEnumerable<string> Reason { get; }
    }
}
