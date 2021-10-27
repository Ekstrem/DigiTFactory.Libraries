using System;
using System.Linq;
using Hive.SeedWorks.Definition;
using Hive.SeedWorks.Invariants;
using Hive.SeedWorks.Monads;

namespace Hive.SeedWorks.Result
{
    /// <summary>
    /// Результат выполнения доменной операции.
    /// </summary>
    public interface IDomainOperationResult
    {
        /// <summary>
        /// Результат операции.
        /// </summary>
        DomainOperationResultEnum Result { get; }
        
        /// <summary>
        /// Причина ошибки.
        /// </summary>
        string Reason { get; }
    }

    public static class Extension
    {
        public static AggregateResult<TBoundedContext> ValidateCommand<TBoundedContext>(
            this BusinessOperationData<TBoundedContext> businessOperationData,
            params IBusinessOperationSpecification<TBoundedContext>[] specifications)
            where TBoundedContext : IBoundedContext
        {
            var invalid = specifications
                .Where(f => f.GetType().GetInterfaces()
                    .Contains(typeof(IBusinessOperationAssertion<TBoundedContext>)))
                .Cast<IBusinessOperationAssertion<TBoundedContext>>()
                .ToArray()
                .PipeTo(asserts =>
                    new AggregateResultWithWarnings<TBoundedContext>(businessOperationData, asserts));
            if (invalid.Result == DomainOperationResultEnum.Exception)
            {
                return invalid;
            }
            
            var exception = specifications
                .Where(f => f.GetType().GetInterfaces()
                    .Contains(typeof(IBusinessOperationValidator<TBoundedContext>)))
                .Cast<IBusinessOperationValidator<TBoundedContext>>()
                .ToArray()
                .PipeTo(asserts =>
                    new AggregateResultWithWarnings<TBoundedContext>(businessOperationData, asserts));

            if (exception.Result == DomainOperationResultEnum.WithWarnings)
            {
                return exception;
            }

            return new AggregateResultSuccess<TBoundedContext>(businessOperationData);
        }
    }
}