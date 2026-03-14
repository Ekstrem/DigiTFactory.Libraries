using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.CommandRepository.Mongo.Configuration;
using DigiTFactory.Libraries.CommandRepository.Mongo.Documents;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace DigiTFactory.Libraries.CommandRepository.Mongo.Repositories
{
    /// <summary>
    /// Стратегия 2: Snapshot + Events (MongoDB).
    /// </summary>
    internal sealed class MongoSnapshotRepository<TBoundedContext, TAnemicModel>
        : IMongoEventStoreRepository<TBoundedContext, TAnemicModel>
        where TBoundedContext : IBoundedContext
        where TAnemicModel : IAnemicModel<TBoundedContext>
    {
        private readonly MongoEventStoreContext _context;
        private readonly MongoEventStoreOptions _options;
        private readonly ILogger _logger;

        public MongoSnapshotRepository(
            MongoEventStoreContext context,
            MongoEventStoreOptions options,
            ILogger<MongoSnapshotRepository<TBoundedContext, TAnemicModel>> logger)
        {
            _context = context;
            _options = options;
            _logger = logger;
        }

        public async Task<List<TAnemicModel>> GetById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogDebug("MongoSnapshot: Getting aggregate {AggregateId}", id);

            var snapshot = await _context.Snapshots
                .Find(x => x.Id == id)
                .SortByDescending(x => x.Version)
                .FirstOrDefaultAsync(cancellationToken);

            if (snapshot != null)
            {
                _logger.LogDebug("MongoSnapshot: Found snapshot at version {Version}", snapshot.Version);

                var eventsAfter = await _context.Events
                    .Find(x => x.Id == id && x.Version > snapshot.Version)
                    .SortBy(x => x.Version)
                    .ToListAsync(cancellationToken);

                _logger.LogDebug("MongoSnapshot: {EventCount} events after snapshot", eventsAfter.Count);

                // TODO: Восстановить из snapshot + события
                return new List<TAnemicModel>();
            }

            var allEvents = await _context.Events
                .Find(x => x.Id == id)
                .SortBy(x => x.Version)
                .ToListAsync(cancellationToken);

            // TODO: Восстановить из полного стрима
            return new List<TAnemicModel>();
        }

        public async Task<TAnemicModel> GetByIdAndVersion(Guid id, long version, CancellationToken cancellationToken)
        {
            var snapshot = await _context.Snapshots
                .Find(x => x.Id == id && x.Version == version)
                .FirstOrDefaultAsync(cancellationToken);

            if (snapshot != null)
                return default!; // TODO: Десериализовать snapshot

            var doc = await _context.Events
                .Find(x => x.Id == id && x.Version == version)
                .FirstOrDefaultAsync(cancellationToken);

            return default!;
        }

        public async Task<TAnemicModel> GetByCorrelationToken(Guid correlationToken, CancellationToken cancellationToken)
        {
            var doc = await _context.Events
                .Find(x => x.CorrelationToken == correlationToken)
                .FirstOrDefaultAsync(cancellationToken);

            return default!;
        }

        public async Task SaveEventAsync(DomainEventDocument document, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "MongoSnapshot: Saving event for aggregate {AggregateId}, version {Version}",
                document.Id, document.Version);

            await _context.Events.InsertOneAsync(document, cancellationToken: cancellationToken);

            var eventCount = await GetEventCountAsync(document.Id, cancellationToken);
            if (eventCount > 0 && eventCount % _options.SnapshotInterval == 0)
            {
                _logger.LogInformation(
                    "MongoSnapshot: Snapshot required at event count {EventCount}", eventCount);
            }
        }

        public async Task SaveSnapshotAsync(Guid id, long version, string aggregateJson, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("MongoSnapshot: Saving snapshot for aggregate {AggregateId} at version {Version}", id, version);

            await _context.Snapshots.InsertOneAsync(new SnapshotDocument
            {
                Id = id,
                Version = version,
                AggregateJson = aggregateJson,
                CreatedAt = DateTime.UtcNow
            }, cancellationToken: cancellationToken);
        }

        public async Task<int> GetEventCountAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var count = await _context.Events.CountDocumentsAsync(x => x.Id == id, cancellationToken: cancellationToken);
            return (int)count;
        }
    }
}
