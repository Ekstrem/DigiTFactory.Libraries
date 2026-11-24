using System.Collections.Generic;
using DigiTFactory.Libraries.CommandRepository.Postgres.Entities;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.CommandRepository.Postgres.Repositories
{
    /// <summary>
    /// Доменный порт Full Event Sourcing: превращает сохранённые события в стрим анемичных моделей.
    /// Реализация специфична для BC и обязана применять события в порядке Version.
    /// </summary>
    public interface IEventStreamRehydrator<TBoundedContext, TAnemicModel>
        where TBoundedContext : IBoundedContext
        where TAnemicModel : IAnemicModel<TBoundedContext>
    {
        /// <summary>Восстанавливает последовательность состояний из сохранённых событий.</summary>
        IReadOnlyList<TAnemicModel> Rehydrate(IReadOnlyList<DomainEventEntry> events);
    }
}
