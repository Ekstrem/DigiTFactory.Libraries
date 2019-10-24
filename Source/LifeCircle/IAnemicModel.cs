using System;
using System.Collections.Generic;

namespace Hive.SeedWorks.LifeCircle
{
    /// <summary>
    /// Анемичная модель ограниченного контекста для Фабрики создания агрегата.
    /// </summary>
    public interface IAnemicModel<TBoundedContext, out TKey>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Корень модели сущности.
        /// </summary>
        IAggregateRoot<TBoundedContext, TKey> Root { get; }

        /// <summary>
        /// Словарь объект значений.
        /// </summary>
        IDictionary<string, IValueObject> ValueObjects { get; }
    }


    /// <summary>
    /// Анемичная модель ограниченного контекста для Фабрики создания агрегата.
    /// </summary>
    public interface IAnemicModel<TBoundedContext> :
        IAnemicModel<TBoundedContext, Guid>
        where TBoundedContext : IBoundedContext
    { }
}