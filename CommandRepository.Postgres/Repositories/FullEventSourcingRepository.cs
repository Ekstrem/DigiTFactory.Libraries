using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.CommandRepository.Postgres.Entities;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DigiTFactory.Libraries.CommandRepository.Postgres.Repositories
{
    /// <summary>
    /// Стратегия 1: Full Event Sourcing.
    /// Все события сохраняются, агрегат восстанавливается из полного стрима.
    /// </summary>
    internal sealed class FullEventSourcingRepository<TBoundedContext, TAnemicModel>
        : IEventStoreRepository<TBoundedContext, TAnemicModel>
        where TBoundedContext : IBoundedContext
        where TAnemicModel : IAnemicModel<TBoundedContext>
    {
        private readonly EventStoreDbContext _dbContext;
        private readonly ILogger _logger;

        public FullEventSourcingRepository(
            EventStoreDbContext dbContext,
            ILogger<FullEventSourcingRepository<TBoundedContext, TAnemicModel>> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<TAnemicModel>> GetById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogDebug("FullES: Getting all events for aggregate {AggregateId}", id);

            var events = await _dbContext.Events
                .Where(e => e.Id == id)
                .OrderBy(e => e.Version)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("FullES: Found {EventCount} events for aggregate {AggregateId}", events.Count, id);

            // TODO: Десериализация событий в анемичные модели через IAggregateFactory
            return new List<TAnemicModel>();
        }

        public async Task<TAnemicModel> GetByIdAndVersion(Guid id, long version, CancellationToken cancellationToken)
        {
            _logger.LogDebug("FullES: Getting event for aggregate {AggregateId} version {Version}", id, version);

            var entry = await _dbContext.Events
                .FirstOrDefaultAsync(e => e.Id == id && e.Version == version, cancellationToken);

            // TODO: Десериализация события в анемичную модель
            return default!;
        }

        public async Task<TAnemicModel> GetByCorrelationToken(Guid correlationToken, CancellationToken cancellationToken)
        {
            _logger.LogDebug("FullES: Getting event by correlation token {CorrelationToken}", correlationToken);

            var entry = await _dbContext.Events
                .FirstOrDefaultAsync(e => e.CorrelationToken == correlationToken, cancellationToken);

            // TODO: Десериализация события в анемичную модель
            return default!;
        }

        public async Task SaveEventAsync(DomainEventEntry entry, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "FullES: Saving event for aggregate {AggregateId}, version {Version}, command {CommandName}",
                entry.Id, entry.Version, entry.CommandName);

            _dbContext.Events.Add(entry);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogDebug("FullES: Event saved for aggregate {AggregateId}", entry.Id);
        }

        public Task SaveSnapshotAsync(Guid id, long version, string aggregateJson, CancellationToken cancellationToken = default)
        {
            // Не используется в стратегии FullEventSourcing
            _logger.LogWarning("FullES: SaveSnapshotAsync called but snapshots are not used in FullEventSourcing strategy");
            return Task.CompletedTask;
        }

        public async Task<int> GetEventCountAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Events.CountAsync(e => e.Id == id, cancellationToken);
        }
    }
}
