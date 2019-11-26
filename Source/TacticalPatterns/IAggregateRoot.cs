using Hive.SeedWorks.Characteristics;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Корень агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контест.</typeparam>
    public interface IAggregateRoot<TBoundedContext> : IHasComplexKey, IValueObject
        where TBoundedContext : IBoundedContext
    {
    }
}
