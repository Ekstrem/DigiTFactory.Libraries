using System;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.CommandRepository.Mongo.Documents;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.CommandRepository.Mongo.Repositories
{
    /// <summary>
    /// Репозиторий MongoDB Event Store с поддержкой сохранения событий и снапшотов.
    /// Расширяет <see cref="IAnemicModelRepository{TBoundedContext, TAnemicModel}"/> из SeedWorks.
    /// </summary>
    public interface IMongoEventStoreRepository<TBoundedContext, TAnemicModel>
        : IAnemicModelRepository<TBoundedContext, TAnemicModel>
        where TBoundedContext : IBoundedContext
        where TAnemicModel : IAnemicModel<TBoundedContext>
    {
        /// <summary>Сохранить доменное событие.</summary>
        Task SaveEventAsync(DomainEventDocument document, CancellationToken cancellationToken = default);

        /// <summary>Сохранить snapshot агрегата.</summary>
        Task SaveSnapshotAsync(Guid id, long version, string aggregateJson, CancellationToken cancellationToken = default);

        /// <summary>Получить количество событий для агрегата.</summary>
        Task<int> GetEventCountAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
