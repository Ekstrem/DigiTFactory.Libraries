using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.CommandRepository.Postgres.Configuration;
using DigiTFactory.Libraries.CommandRepository.Postgres.Entities;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DigiTFactory.Libraries.CommandRepository.Postgres.Repositories
{
    /// <summary>
    /// Стратегия 2: Snapshot + Events.
    /// Каждые N событий сохраняется snapshot агрегата в отдельную таблицу.
    /// При чтении: последний snapshot + события после него.
    /// </summary>
    internal sealed class SnapshotRepository<TBoundedContext, TAnemicModel>
        : IEventStoreRepository<TBoundedContext, TAnemicModel>
        where TBoundedContext : IBoundedContext
        where TAnemicModel : IAnemicModel<TBoundedContext>
    {
        private readonly EventStoreDbContext _dbContext;
        private readonly EventStoreOptions _options;
        private readonly ILogger _logger;

        public SnapshotRepository(
            EventStoreDbContext dbContext,
            EventStoreOptions options,
            ILogger<SnapshotRepository<TBoundedContext, TAnemicModel>> logger)
        {
            _dbContext = dbContext;
            _options = options;
            _logger = logger;
        }

        public async Task<List<TAnemicModel>> GetById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Snapshot: Getting aggregate {AggregateId} with snapshot optimization", id);

            // Найти последний snapshot
            var snapshot = await _dbContext.Snapshots
                .Where(s => s.Id == id)
                .OrderByDescending(s => s.Version)
                .FirstOrDefaultAsync(cancellationToken);

            if (snapshot != null)
            {
                _logger.LogDebug(
                    "Snapshot: Found snapshot for aggregate {AggregateId} at version {Version}",
                    id, snapshot.Version);

                // Загрузить только события после snapshot
                var eventsAfterSnapshot = await _dbContext.Events
                    .Where(e => e.Id == id && e.Version > snapshot.Version)
                    .OrderBy(e => e.Version)
                    .ToListAsync(cancellationToken);

                _logger.LogDebug(
                    "Snapshot: Found {EventCount} events after snapshot for aggregate {AggregateId}",
                    eventsAfterSnapshot.Count, id);

                // TODO: Восстановить агрегат из snapshot + применить события
                return new List<TAnemicModel>();
            }

            // Нет snapshot — загружаем все события
            _logger.LogDebug("Snapshot: No snapshot found for aggregate {AggregateId}, loading all events", id);

            var allEvents = await _dbContext.Events
                .Where(e => e.Id == id)
                .OrderBy(e => e.Version)
                .ToListAsync(cancellationToken);

            // TODO: Восстановить агрегат из полного стрима событий
            return new List<TAnemicModel>();
        }

        public async Task<TAnemicModel> GetByIdAndVersion(Guid id, long version, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Snapshot: Getting aggregate {AggregateId} version {Version}", id, version);

            // Проверить, есть ли snapshot с этой версией
            var snapshot = await _dbContext.Snapshots
                .FirstOrDefaultAsync(s => s.Id == id && s.Version == version, cancellationToken);

            if (snapshot != null)
            {
                // TODO: Десериализовать snapshot
                return default!;
            }

            // Fallback на событие
            var entry = await _dbContext.Events
                .FirstOrDefaultAsync(e => e.Id == id && e.Version == version, cancellationToken);

            // TODO: Десериализация события в анемичную модель
            return default!;
        }

        public async Task<TAnemicModel> GetByCorrelationToken(Guid correlationToken, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Snapshot: Getting event by correlation token {CorrelationToken}", correlationToken);

            var entry = await _dbContext.Events
                .FirstOrDefaultAsync(e => e.CorrelationToken == correlationToken, cancellationToken);

            // TODO: Десериализация события в анемичную модель
            return default!;
        }

        public async Task SaveEventAsync(DomainEventEntry entry, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Snapshot: Saving event for aggregate {AggregateId}, version {Version}, command {CommandName}",
                entry.Id, entry.Version, entry.CommandName);

            _dbContext.Events.Add(entry);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Проверить, нужен ли snapshot
            var eventCount = await GetEventCountAsync(entry.Id, cancellationToken);
            if (eventCount > 0 && eventCount % _options.SnapshotInterval == 0)
            {
                _logger.LogInformation(
                    "Snapshot: Event count {EventCount} reached interval {Interval} for aggregate {AggregateId}. Snapshot required.",
                    eventCount, _options.SnapshotInterval, entry.Id);

                // TODO: Вызывающий код должен вызвать SaveSnapshotAsync с сериализованным агрегатом
                // Библиотека не знает как сериализовать конкретный агрегат — это ответственность микросервиса
            }

            _logger.LogDebug("Snapshot: Event saved for aggregate {AggregateId}", entry.Id);
        }

        public async Task SaveSnapshotAsync(Guid id, long version, string aggregateJson, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Snapshot: Saving snapshot for aggregate {AggregateId} at version {Version}", id, version);

            var snapshot = new SnapshotEntry
            {
                Id = id,
                Version = version,
                AggregateJson = aggregateJson,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Snapshots.Add(snapshot);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogDebug("Snapshot: Snapshot saved for aggregate {AggregateId}", id);
        }

        public async Task<int> GetEventCountAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Events.CountAsync(e => e.Id == id, cancellationToken);
        }
    }
}
