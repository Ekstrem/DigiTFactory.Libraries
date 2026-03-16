using System.Collections.Generic;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.DynamicTypes
{
    /// <summary>
    /// Динамическая область ограниченного контекста, построенная из метаданных.
    /// </summary>
    public sealed class DynamicBoundedContextScope<TBoundedContext> : IBoundedContextScope<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Создание scope из набора операций и валидаторов.
        /// </summary>
        public DynamicBoundedContextScope(
            IReadOnlyDictionary<string, IAggregateBusinessOperation<TBoundedContext>> operations,
            IReadOnlyList<IBusinessEntityValidator<TBoundedContext>> validators)
        {
            Operations = operations;
            Validators = validators;
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, IAggregateBusinessOperation<TBoundedContext>> Operations { get; }

        /// <inheritdoc />
        public IReadOnlyList<IBusinessEntityValidator<TBoundedContext>> Validators { get; }
    }
}
