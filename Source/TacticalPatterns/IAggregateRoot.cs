using Hive.SeedWorks.Characteristics;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Корень агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контест.</typeparam>
    public interface IAggregateRoot<TBoundedContext> : IComplexKey, IValueObject
        where TBoundedContext : IBoundedContext
    {
    }
}
