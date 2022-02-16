using Hive.SeedWorks.Definition;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Корень агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контест.</typeparam>
    public interface IAggregateRoot<TBoundedContext> : IValueObject
        where TBoundedContext : IBoundedContext
    {
    }
}
