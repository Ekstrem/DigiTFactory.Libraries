using Hive.SeedWorks.Characteristics;

namespace Hive.SeedWorks.TacticalPatterns.Abstracts
{

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
