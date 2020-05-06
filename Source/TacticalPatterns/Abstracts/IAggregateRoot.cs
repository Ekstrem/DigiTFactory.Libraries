namespace Hive.SeedWorks.TacticalPatterns.Abstracts
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
