using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.CommandRepository.Mongo.Configuration;
using DigiTFactory.Libraries.CommandRepository.Mongo.Documents;
using MongoDB.Driver;

namespace DigiTFactory.Libraries.CommandRepository.Mongo
{
    /// <summary>
    /// Контекст MongoDB Event Store. Предоставляет коллекции и создаёт индексы.
    /// </summary>
    public class MongoEventStoreContext
    {
        private readonly IMongoDatabase _database;
        private readonly MongoEventStoreOptions _options;

        public MongoEventStoreContext(IMongoClient client, MongoEventStoreOptions options)
        {
            _options = options;
            _database = client.GetDatabase(options.DatabaseName);
        }

        /// <summary>Коллекция доменных событий.</summary>
        public IMongoCollection<DomainEventDocument> Events
            => _database.GetCollection<DomainEventDocument>(_options.CollectionPrefix + "DomainEvents");

        /// <summary>Коллекция снапшотов.</summary>
        public IMongoCollection<SnapshotDocument> Snapshots
            => _database.GetCollection<SnapshotDocument>(_options.CollectionPrefix + "Snapshots");

        /// <summary>Коллекция состояний агрегатов.</summary>
        public IMongoCollection<AggregateStateDocument> AggregateStates
            => _database.GetCollection<AggregateStateDocument>(_options.CollectionPrefix + "AggregateStates");

        /// <summary>
        /// Создаёт индексы для всех коллекций. Вызывается один раз при старте.
        /// </summary>
        public async Task EnsureIndexesAsync(CancellationToken cancellationToken = default)
        {
            // Events: составной индекс (Id, Version), индексы на CorrelationToken и CreatedAt
            var eventsIndexes = Events.Indexes;
            await eventsIndexes.CreateManyAsync(new[]
            {
                new CreateIndexModel<DomainEventDocument>(
                    Builders<DomainEventDocument>.IndexKeys
                        .Ascending(x => x.Id)
                        .Ascending(x => x.Version),
                    new CreateIndexOptions { Unique = true, Name = "IX_Id_Version" }),
                new CreateIndexModel<DomainEventDocument>(
                    Builders<DomainEventDocument>.IndexKeys.Ascending(x => x.CorrelationToken),
                    new CreateIndexOptions { Name = "IX_CorrelationToken" }),
                new CreateIndexModel<DomainEventDocument>(
                    Builders<DomainEventDocument>.IndexKeys.Ascending(x => x.CreatedAt),
                    new CreateIndexOptions { Name = "IX_CreatedAt" })
            }, cancellationToken);

            // Snapshots: составной индекс (Id, Version)
            await Snapshots.Indexes.CreateOneAsync(
                new CreateIndexModel<SnapshotDocument>(
                    Builders<SnapshotDocument>.IndexKeys
                        .Ascending(x => x.Id)
                        .Ascending(x => x.Version),
                    new CreateIndexOptions { Unique = true, Name = "IX_Id_Version" }),
                cancellationToken: cancellationToken);
        }
    }
}
