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
    /// Стратегия 3: State Only (MongoDB). UPSERT агрегата целиком.
    /// </summary>
    internal sealed class MongoStateOnlyRepository<TBoundedContext, TAnemicModel>
        : IMongoEventStoreRepository<TBoundedContext, TAnemicModel>
        where TBoundedContext : IBoundedContext
        where TAnemicModel : IAnemicModel<TBoundedContext>
    {
        private readonly MongoEventStoreContext _context;
        private readonly ILogger _logger;

        public MongoStateOnlyRepository(
            MongoEventStoreContext context,
            ILogger<MongoStateOnlyRepository<TBoundedContext, TAnemicModel>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TAnemicModel>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var state = await _context.AggregateStates
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (state == null)
                return new List<TAnemicModel>();

            // TODO: Десериализовать AggregateJson
            return new List<TAnemicModel>();
        }

        public async Task<TAnemicModel> GetByIdAndVersion(Guid id, long version, CancellationToken cancellationToken)
        {
            var state = await _context.AggregateStates
                .Find(x => x.Id == id && x.Version == version)
                .FirstOrDefaultAsync(cancellationToken);

            return default!;
        }

        public async Task<TAnemicModel> GetByCorrelationToken(Guid correlationToken, CancellationToken cancellationToken)
        {
            // StateOnly не хранит CorrelationToken в AggregateStates
            var eventDoc = await _context.Events
                .Find(x => x.CorrelationToken == correlationToken)
                .FirstOrDefaultAsync(cancellationToken);

            if (eventDoc == null)
                return default!;

            return await GetByIdAndVersion(eventDoc.Id, eventDoc.Version, cancellationToken);
        }

        public async Task SaveEventAsync(DomainEventDocument document, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "MongoStateOnly: Saving aggregate state for {AggregateId}, version {Version}",
                document.Id, document.Version);

            var filter = Builders<AggregateStateDocument>.Filter.Eq(x => x.Id, document.Id);
            var replacement = new AggregateStateDocument
            {
                Id = document.Id,
                Version = document.Version,
                AggregateJson = document.ChangedValueObjectsJson,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.AggregateStates.ReplaceOneAsync(
                filter,
                replacement,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }

        public Task SaveSnapshotAsync(Guid id, long version, string aggregateJson, CancellationToken cancellationToken = default)
        {
            _logger.LogWarning("MongoStateOnly: Snapshots not used in StateOnly strategy");
            return Task.CompletedTask;
        }

        public Task<int> GetEventCountAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }
    }
}
