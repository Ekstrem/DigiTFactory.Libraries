using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using Hive.SeedWorks.Definition;
using Hive.SeedWorks.Invariants;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.SeedWorks.Operations
{
    /// <summary>
    /// Служебный класс валидирования операций.
    /// </summary>
    /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
    /// <typeparam name="TResults">Результат выполнения бизнес-операции.</typeparam>
    public class BusinessOperationValidator<TBoundedContext, TResults>
        where TBoundedContext: IBoundedContext
    {
        private readonly Dictionary<IBusinessOperationSpecification<TBoundedContext, TResults>, bool> _validators;

        internal protected BusinessOperationValidator(
            BusinessOperationData<TBoundedContext> businessOperationData,
            params IBusinessOperationSpecification<TBoundedContext, TResults>[] specifications)
        {
            _validators = specifications
                .ToDictionary(
                    k => k,
                    v => v.IsSatisfiedBy(businessOperationData));
        }

        public bool Result => _validators.Values.All(p => p);

        public IDictionary<string, TResults> GetFailedValidatorsReasons()
            => _validators
                .Where(f => !f.Value)
                .ToDictionary(
                    k => k.Key.Reason,
                    v => v.Key.DomainResult);
    }


    /// <summary>
    /// Служебный класс валидирования операций.
    /// </summary>
    /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
    /// <typeparam name="TResults">Результат выполнения бизнес-операции.</typeparam>
    public class BusinessOperationValidator<TBoundedContext> :
        BusinessOperationValidator<TBoundedContext, DomainOperationResultEnum>
        where TBoundedContext : IBoundedContext
    {
        internal BusinessOperationValidator(
            BusinessOperationData<TBoundedContext> businessOperationData,
            params IBusinessOperationSpecification<TBoundedContext, DomainOperationResultEnum>[] specifications)
            : base(businessOperationData, specifications)
        { }
    }
}