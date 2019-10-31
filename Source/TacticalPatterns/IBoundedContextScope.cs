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
        IList<IAggregateBusinessOperationFactory<TBoundedContext>> Operations { get; }

        IList<IBusinessValidator<TBoundedContext>> Validators { get; }
    }
}