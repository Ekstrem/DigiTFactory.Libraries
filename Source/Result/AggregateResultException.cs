using System.Collections.Generic;
using System.Linq;
using Hive.SeedWorks.Definition;
using Hive.SeedWorks.Invariants;
using Hive.SeedWorks.Operations;

namespace Hive.SeedWorks.Result
{
    /// <summary>
    /// Рузультат выполнения операции.
    /// Операция не выполнена.
    /// Следует не заполнять объекты значения, но указать причину ошибки.
    /// </summary>
    /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
    public class AggregateResultException<TBoundedContext> :
        AggregateResult<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly BusinessOperationValidator<TBoundedContext> _assertions;

        public AggregateResultException(
            BusinessOperationData<TBoundedContext> businessOperationData,
            params IBusinessOperationSpecification<TBoundedContext>[] specifications)
            : base(businessOperationData)
        {
            _assertions = new BusinessOperationValidator<TBoundedContext>(
                businessOperationData, specifications);
        }

        public sealed override DomainOperationResultEnum Result => DomainOperationResultEnum.Exception;

        public sealed override IEnumerable<string> Reason
            => _assertions
                .GetFailedValidatorsReasons()
                .Where(f => f.Value == DomainOperationResultEnum.Exception)
                .Select(m => m.Key);
    }
}