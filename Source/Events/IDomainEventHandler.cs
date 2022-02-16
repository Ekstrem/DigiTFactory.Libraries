using Hive.SeedWorks.Definition;

namespace Hive.SeedWorks.Events
{
    /// <summary>
    /// Обработчик доменных событий.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public interface IDomainEventHandler<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
    }
}