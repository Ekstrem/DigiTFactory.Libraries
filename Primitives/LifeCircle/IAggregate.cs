using System;
using System.Collections.Generic;
using Hive.SeedWorks.Business;

namespace Hive.SeedWorks.LifeCircle
{
    /// <summary>
    /// Агрегат.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контест.</typeparam>
    public interface IAggregate<TBoundedContext> : 
        IAggregateRoot<TBoundedContext>,
        IAggregate<TBoundedContext, Guid>
        where TBoundedContext : IBoundedContext
    {
    }

    /// <summary>
    /// Агрегат.
    /// </summary>
    /// <typeparam name="TKey">Тип ключевого поля.</typeparam>
    /// <typeparam name="TBoundedContext">Ограниченный контест.</typeparam>
    public interface IAggregate<TBoundedContext, out TKey> :
        IAggregateRoot<TBoundedContext, TKey>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Информация о версионности агрегата.
        /// </summary>
        IHasVersion Version { get; }

        /// <summary>
        /// Словарь объект значений.
        /// </summary>
        IDictionary<string, IValueObject> ValueObjects { get; }
    }
}
