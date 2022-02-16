using Hive.SeedWorks.Definition;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Агрегат.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контест.</typeparam>
    /// <typeparam name="TKey">Тип улючевого поля.</typeparam>
    public interface IAggregate<TBoundedContext, out TKey> :
        IAnemicModel<TBoundedContext, TKey>
        where TBoundedContext : IBoundedContext
    {
    }
    
    /// <summary>
    /// Агрегат.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контест.</typeparam>
    public interface IAggregate<TBoundedContext> :
        IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
    }
}
