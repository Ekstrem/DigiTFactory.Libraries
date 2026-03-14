using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.CommandRepository.Mongo.Documents;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace DigiTFactory.Libraries.CommandRepository.Mongo.Repositories
{
    /// <summary>
    /// Стратегия 1: Full Event Sourcing (MongoDB).
    /// </summary>
    internal sealed class MongoFullEventSourcingRepository<TBoundedContext, TAnemicModel>
        : IMongoEventStoreRepository<TBoundedContext, TAnemicModel>
        where TBoundedContext : IBoundedContext
        where TAnemicModel : IAnemicModel<TBoundedContext>
    {
        private readonly MongoEventStoreContext _context;
        private readonly ILogger _logger;

        public MongoFullEventSourcingRepository(
            MongoEventStoreContext context,
            ILogger<MongoFullEventSourcingRepository<TBoundedContext, TAnemicModel>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TAnemicModel>> GetById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogDebug("MongoFullES: Getting all events for aggregate {AggregateId}", id);

            var events = await _context.Events
                .Find(x => x.Id == id)
                .SortBy(x => x.Version)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("MongoFullES: Found {EventCount} events for aggregate {AggregateId}", events.Count, id);

            // TODO: Десериализация событий в анемичные модели
            return new List<TAnemicModel>();
        }

        public async Task<TAnemicModel> GetByIdAndVersion(Guid id, long version, CancellationToken cancellationToken)
        {
            var doc = await _context.Events
                .Find(x => x.Id == id && x.Version == version)
                .FirstOrDefaultAsync(cancellationToken);

            // TODO: Десериализация
            return default!;
        }

        public async Task<TAnemicModel> GetByCorrelationToken(Guid correlationToken, CancellationToken cancellationToken)
        {
            var doc = await _context.Events
                .Find(x => x.CorrelationToken == correlationToken)
                .FirstOrDefaultAsync(cancellationToken);

            // TODO: Десериализация
            return default!;
        }

        public async Task SaveEventAsync(DomainEventDocument document, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "MongoFullES: Saving event for aggregate {AggregateId}, version {Version}, command {CommandName}",
                document.Id, document.Version, document.CommandName);

            await _context.Events.InsertOneAsync(document, cancellationToken: cancellationToken);
        }

        public Task SaveSnapshotAsync(Guid id, long version, string aggregateJson, CancellationToken cancellationToken = default)
        {
            _logger.LogWarning("MongoFullES: Snapshots not used in FullEventSourcing strategy");
            return Task.CompletedTask;
        }

        public async Task<int> GetEventCountAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var count = await _context.Events.CountDocumentsAsync(x => x.Id == id, cancellationToken: cancellationToken);
            return (int)count;
        }
    }
}
