using System;

namespace Hive.SeedWorks.LifeCircle
{
    /// <summary>
    /// Корень агрегата.
    /// </summary>
    /// <typeparam name="TKey">Тип ключевого поля.</typeparam>
    /// <typeparam name="TBoundedContext">Ограниченный контест.</typeparam>
    public interface IAggregateRoot<TBoundedContext, out TKey> :
        IEntity<TKey>
        where TBoundedContext : IBoundedContext
    {
    }

    /// <summary>
    /// Корень агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контест.</typeparam>
    public interface IAggregateRoot<TBoundedContext> :
        IAggregateRoot<TBoundedContext, Guid>
        where TBoundedContext : IBoundedContext
    {
    }
}