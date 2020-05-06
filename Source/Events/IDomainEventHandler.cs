using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

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