using System.Collections.Generic;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Границы ограниченного контекста.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public interface IBoundedContextScope<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Бизнес-операции - фабрики.
        /// </summary>
        IReadOnlyDictionary<string, IAggregateBusinessOperationFactory<TBoundedContext>> Operations { get; }

        /// <summary>
        /// Валидаторы модели.
        /// </summary>
        IReadOnlyList<IBusinessValidator<TBoundedContext>> Validators { get; }
    }
}
