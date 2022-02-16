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
    /// <typeparam name="TModel">Тип анемичной модели.</typeparam>
    public class BusinessOperationValidator<TBoundedContext, TModel, TResults>
        where TModel : IAnemicModel<TBoundedContext>
        where TBoundedContext: IBoundedContext
    {
        private readonly Dictionary<IBusinessOperationSpecification<TBoundedContext, TModel, TResults>, bool> _validators;

        internal protected BusinessOperationValidator(
            BusinessOperationData<TBoundedContext, TModel> businessOperationData,
            params IBusinessOperationSpecification<TBoundedContext, TModel, TResults>[] specifications)
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
    /// <typeparam name="TModel">Тип анемичной модели.</typeparam>
    public class BusinessOperationValidator<TBoundedContext, TModel> :
        BusinessOperationValidator<TBoundedContext, TModel, DomainOperationResultEnum>
        where TModel : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        internal BusinessOperationValidator(
            BusinessOperationData<TBoundedContext, TModel> businessOperationData,
            params IBusinessOperationSpecification<TBoundedContext, TModel, DomainOperationResultEnum>[] specifications)
            : base(businessOperationData, specifications)
        { }
    }
}