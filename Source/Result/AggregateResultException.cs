using System.Collections.Generic;
using System.Linq;
using Hive.SeedWorks.Definition;
using Hive.SeedWorks.Invariants;
using Hive.SeedWorks.Operations;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.SeedWorks.Result
{
    /// <summary>
    /// Рузультат выполнения операции.
    /// Операция не выполнена.
    /// Следует не заполнять объекты значения, но указать причину ошибки.
    /// </summary>
    /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
    /// <typeparam name="TModel">Тип анемичной модели.</typeparam>
    public class AggregateResultException<TBoundedContext, TModel> :
        AggregateResult<TBoundedContext, TModel>
        where TModel : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly BusinessOperationValidator<TBoundedContext, TModel> _assertions;

        public AggregateResultException(
            BusinessOperationData<TBoundedContext, TModel> businessOperationData,
            params IBusinessOperationSpecification<TBoundedContext, TModel>[] specifications)
            : base(businessOperationData)
            => _assertions = new BusinessOperationValidator<TBoundedContext, TModel>(
                businessOperationData, specifications);

        public sealed override DomainOperationResultEnum Result
            => _assertions.Result
                ? DomainOperationResultEnum.Success
                : DomainOperationResultEnum.Exception;

        public sealed override IEnumerable<string> Reason
            => _assertions
                .GetFailedValidatorsReasons()
                .Where(f => f.Value == DomainOperationResultEnum.Exception)
                .Select(m => m.Key);
    }
}