using System.Collections.Generic;
using DigiTFactory.Libraries.SeedWorks.Definition;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Область ограниченного контекста.
    /// Определяет набор бизнес-операций и валидаторов для агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public interface IBoundedContextScope<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Бизнес-операции агрегата.
        /// </summary>
        IReadOnlyDictionary<string, IAggregateBusinessOperation<TBoundedContext>> Operations { get; }

        /// <summary>
        /// Валидаторы бизнес-сущностей.
        /// </summary>
        IReadOnlyList<IBusinessEntityValidator<TBoundedContext>> Validators { get; }
    }
}
