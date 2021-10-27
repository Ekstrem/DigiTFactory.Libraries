using System.Collections.Generic;
using System.Linq;
using Hive.SeedWorks.Definition;
using Hive.SeedWorks.Invariants;
using Hive.SeedWorks.Operations;

namespace Hive.SeedWorks.Result
{
    /// <summary>
    /// Результат выполнения бизнес-операции в агрегате.
    /// Операция завершилась, но необходимо отреагировать на несоблюдение инвариантов.
    /// Следует отразить изменившиеся объекты значения, заполнить причину нарушения инварианта.
    /// </summary>
    /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
    public class AggregateResultWithWarnings<TBoundedContext> :
        AggregateResult<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly BusinessOperationValidator<TBoundedContext> _assertions;

        public AggregateResultWithWarnings(
            BusinessOperationData<TBoundedContext> businessOperationData,
            params IBusinessOperationSpecification<TBoundedContext>[] specifications)
            : base(businessOperationData)
        {
            _assertions = new BusinessOperationValidator<TBoundedContext>(
                businessOperationData, specifications);
        }

        public sealed override DomainOperationResultEnum Result => DomainOperationResultEnum.WithWarnings;

        public sealed override IEnumerable<string> Reason
            => _assertions
                .GetFailedValidatorsReasons()
                .Where(f => f.Value == DomainOperationResultEnum.WithWarnings)
                .Select(m => m.Key);
    }
}