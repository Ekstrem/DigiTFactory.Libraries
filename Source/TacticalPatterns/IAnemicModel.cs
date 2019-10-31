using System.Collections.Generic;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Анемичная модель ограниченного контекста для Фабрики создания агрегата.
    /// </summary>
    public interface IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Корень модели сущности.
        /// </summary>
        IAggregateRoot<TBoundedContext> Root { get; }

        /// <summary>
        /// Словарь объект значений.
        /// </summary>
        IDictionary<string, IValueObject> ValueObjects { get; }
    }
}