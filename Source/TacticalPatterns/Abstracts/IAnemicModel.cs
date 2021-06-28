using System.Collections.Generic;
using Hive.SeedWorks.Characteristics;

namespace Hive.SeedWorks.TacticalPatterns.Abstracts
{
    public interface IAnemicModel :
        IHasComplexKey,
        IHasCommandMetadata
    {
        /// <summary>
        /// Имя ограниченного контекста.
        /// </summary>
        string BoundedContextName { get; }
        
        /// <summary>
        /// Объекты значения.
        /// </summary>
        IDictionary<string, IValueObject> ValueObjects { get; }
    }
    
    
    /// <summary>
    /// Анемичная модель ограниченного контекста для Фабрики создания агрегата.
    /// Анемичная модель должна содержать объект-значения, реализующие интерфейс <see cref="IValueObject"/>.
    /// </summary>
    public interface IAnemicModel<TBoundedContext> :
        IAnemicModel
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
