using System.Collections.Generic;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Анемичная модель ограниченного контекста для Фабрики создания агрегата.
    /// Анемичная модель должна содержать объект-значения, реализующие интерфейс <see cref="IValueObject"/>.
    /// </summary>
    public interface IAnemicModel<TBoundedContext> :
        IAggregateRoot<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Словарь объект значений.
        /// </summary>
        IDictionary<string, IValueObject> ValueObjects { get; }
    }
}
