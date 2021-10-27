using System;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.Definition;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Анемичная модель ограниченного контекста для Фабрики создания агрегата.
    /// Анемичная модель должна содержать объект-значения, реализующие интерфейс <see cref="IValueObject"/>.
    /// </summary>
    public interface IAnemicModel<TBoundedContext, out TKey> :
        IHasKey<TKey>,
        IHasVersion,
        ICommandSubject,
        IHasCorrelationToken,
        IHasValueObjects
        where TBoundedContext : IBoundedContext
    {
    }
    
    /// <summary>
    /// Анемичная модель ограниченного контекста для Фабрики создания агрегата.
    /// Анемичная модель должна содержать объект-значения, реализующие интерфейс <see cref="IValueObject"/>.
    /// </summary>
    public interface IAnemicModel<TBoundedContext> :
        IAnemicModel<TBoundedContext, Guid>
        where TBoundedContext : IBoundedContext
    {
    }
}
