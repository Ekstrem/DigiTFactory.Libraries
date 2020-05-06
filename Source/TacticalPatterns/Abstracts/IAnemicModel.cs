using System.Collections.Generic;
using Hive.SeedWorks.Characteristics;

namespace Hive.SeedWorks.TacticalPatterns.Abstracts
{
    public interface IHasCommandMetadata
    {
        /// <summary>
        /// Метаданные о предыдущей операции,
        /// породившей данную версию.
        /// </summary>
        ICommandMetadata PreviousOperation { get; }
    }

    /// <summary>
    /// Анемичная модель ограниченного контекста для Фабрики создания агрегата.
    /// Анемичная модель должна содержать объект-значения, реализующие интерфейс <see cref="IValueObject"/>.
    /// </summary>
    public interface IAnemicModel<TBoundedContext> : 
        IHasCommandMetadata
        where TBoundedContext : IBoundedContext
    {
    }
    
    
    /// <summary>
    /// Анемичная модель ограниченного контекста для Фабрики создания агрегата.
    /// Анемичная модель должна содержать объект-значения, реализующие интерфейс <see cref="IValueObject"/>.
    /// </summary>
    public interface IAnemicModel<TBoundedContext, out TRoot> :
        IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
        where TRoot : IAggregateRoot<TBoundedContext>
    {
        /// <summary>
        /// Корень агрегата как фиксированное объект-значение,
        /// с заданным именем.
        /// </summary>
        TRoot Root { get; }
    }
}
