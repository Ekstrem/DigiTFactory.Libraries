using System.Collections.Generic;
using System.Linq;
using Hive.SeedWorks.Definition;
using Hive.SeedWorks.Invariants;
using Hive.SeedWorks.Operations;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.SeedWorks.Result
{
    /// <summary>
    /// Результат выполнения бизнес-операции в агрегате.
    /// Операция завершилась, но необходимо отреагировать на несоблюдение инвариантов.
    /// Следует отразить изменившиеся объекты значения, заполнить причину нарушения инварианта.
    /// </summary>
    /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
    /// <typeparam name="TModel">Тип анемичной модели.</typeparam>
    public class AggregateResultWithWarnings<TBoundedContext, TModel> :
        AggregateResult<TBoundedContext, TModel>
        where TModel : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly BusinessOperationValidator<TBoundedContext, TModel> _assertions;

        public AggregateResultWithWarnings(
            BusinessOperationData<TBoundedContext, TModel> businessOperationData,
            params IBusinessOperationSpecification<TBoundedContext, TModel>[] specifications)
            : base(businessOperationData)
            => _assertions = new BusinessOperationValidator<TBoundedContext, TModel>(businessOperationData, specifications);

        public sealed override DomainOperationResultEnum Result
            => _assertions.Result
                ? DomainOperationResultEnum.Success
                : DomainOperationResultEnum.WithWarnings;

        public sealed override IEnumerable<string> Reason
            => _assertions
                .GetFailedValidatorsReasons()
                .Where(f => f.Value == DomainOperationResultEnum.WithWarnings)
                .Select(m => m.Key);
    }
}