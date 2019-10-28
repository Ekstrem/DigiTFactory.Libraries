using System;
using Hive.SeedWorks.Business;

namespace Hive.SeedWorks.LifeCircle
{
    /// <summary>
    /// Корень агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контест.</typeparam>
    public interface IAggregateRoot<TBoundedContext> : IHasComplexKey
        where TBoundedContext : IBoundedContext
    {
    }
}