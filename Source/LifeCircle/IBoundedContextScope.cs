using System.Collections.Generic;

namespace Hive.SeedWorks.LifeCircle
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
        IList<IAggregateBusinessOperationFactory<IAnemicModel<TBoundedContext>, TBoundedContext>> Operations { get; }

        /// <summary>
        /// Валидатор модели предметной области.
        /// </summary>
        IList<IBusinessValidator<TBoundedContext>> Validators { get; }
    }
}