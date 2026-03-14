using System;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.CommandRepository.Postgres.Entities;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.CommandRepository.Postgres.Repositories
{
    /// <summary>
    /// Репозиторий Event Store с поддержкой сохранения событий и снапшотов.
    /// Расширяет <see cref="IAnemicModelRepository{TBoundedContext, TAnemicModel}"/> из SeedWorks.
    /// </summary>
    /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
    /// <typeparam name="TAnemicModel">Тип анемичной модели.</typeparam>
    public interface IEventStoreRepository<TBoundedContext, TAnemicModel>
        : IAnemicModelRepository<TBoundedContext, TAnemicModel>
        where TBoundedContext : IBoundedContext
        where TAnemicModel : IAnemicModel<TBoundedContext>
    {
        /// <summary>
        /// Сохранить доменное событие.
        /// В зависимости от стратегии может также создать snapshot или обновить состояние агрегата.
        /// </summary>
        Task SaveEventAsync(DomainEventEntry entry, CancellationToken cancellationToken = default);

        /// <summary>
        /// Сохранить snapshot агрегата вручную (для стратегии SnapshotAfterN).
        /// </summary>
        Task SaveSnapshotAsync(Guid id, long version, string aggregateJson, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить количество событий для агрегата.
        /// </summary>
        Task<int> GetEventCountAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
