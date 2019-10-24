using Hive.SeedWorks.Business;
using System;

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
        IHasKey<TKey>,
        IAnemicModel<TBoundedContext, TKey>,
        IBoundedContextScope<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        IHasVersion Version { get; }
    }
}
