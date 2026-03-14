using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Cassandra;
using DigiTFactory.Libraries.ReadRepository.Scylla.Configuration;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository;
using Microsoft.Extensions.Logging;

namespace DigiTFactory.Libraries.ReadRepository.Scylla
{
    /// <summary>
    /// ScyllaDB/Cassandra реализация IReadModelStore.
    /// Используется проекциями для записи/обновления Read-моделей.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TReadModel">Тип Read-модели.</typeparam>
    public class ScyllaReadModelStore<TBoundedContext, TReadModel>
        : IReadModelStore<TBoundedContext, TReadModel>
        where TBoundedContext : IBoundedContext
        where TReadModel : class, IReadModel<TBoundedContext>
    {
        private readonly ISession _session;
        private readonly ScyllaReadStoreOptions _options;
        private readonly ILogger<ScyllaReadModelStore<TBoundedContext, TReadModel>> _logger;
        private readonly string _modelType;

        public ScyllaReadModelStore(
            ISession session,
            ScyllaReadStoreOptions options,
            ILogger<ScyllaReadModelStore<TBoundedContext, TReadModel>> logger)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _modelType = typeof(TReadModel).Name;
        }

        /// <inheritdoc />
        public async Task UpsertAsync(TReadModel model, CancellationToken cancellationToken = default)
        {
            var id = GetIdFromModel(model);
            var json = JsonSerializer.Serialize(model);

            var cql = $"INSERT INTO {_options.Keyspace}.{_options.TableName} " +
                       "(id, data, model_type, updated_at) VALUES (?, ?, ?, toTimestamp(now()))";

            var statement = new SimpleStatement(cql, id, json, _modelType);
            await _session.ExecuteAsync(statement);

            _logger.LogDebug("Upserted {ModelType} with id {Id} in ScyllaDB", _modelType, id);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var cql = $"DELETE FROM {_options.Keyspace}.{_options.TableName} WHERE id = ?";

            var statement = new SimpleStatement(cql, id);
            await _session.ExecuteAsync(statement);

            _logger.LogDebug("Deleted {ModelType} with id {Id} from ScyllaDB", _modelType, id);
        }

        private static Guid GetIdFromModel(TReadModel model)
        {
            var idProperty = typeof(TReadModel).GetProperty("Id");
            if (idProperty == null)
                throw new InvalidOperationException(
                    $"Read model {typeof(TReadModel).Name} must have an 'Id' property of type Guid.");

            return (Guid)idProperty.GetValue(model)!;
        }
    }
}
