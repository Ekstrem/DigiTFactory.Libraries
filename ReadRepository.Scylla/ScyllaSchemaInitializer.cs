using System.Threading;
using System.Threading.Tasks;
using Cassandra;
using DigiTFactory.Libraries.ReadRepository.Scylla.Configuration;
using Microsoft.Extensions.Logging;

namespace DigiTFactory.Libraries.ReadRepository.Scylla
{
    /// <summary>
    /// Инициализация схемы ScyllaDB (keyspace и таблица) при старте.
    /// </summary>
    internal sealed class ScyllaSchemaInitializer
    {
        private readonly ScyllaReadStoreOptions _options;
        private readonly ILogger<ScyllaSchemaInitializer> _logger;

        public ScyllaSchemaInitializer(
            ScyllaReadStoreOptions options,
            ILogger<ScyllaSchemaInitializer> logger)
        {
            _options = options;
            _logger = logger;
        }

        public async Task InitializeAsync(ISession session, CancellationToken ct = default)
        {
            if (!_options.AutoCreateSchema)
                return;

            var createKeyspace = $@"
                CREATE KEYSPACE IF NOT EXISTS {_options.Keyspace}
                WITH replication = {{
                    'class': '{_options.ReplicationStrategy}',
                    'replication_factor': {_options.ReplicationFactor}
                }}";

            await session.ExecuteAsync(new SimpleStatement(createKeyspace));

            var createTable = $@"
                CREATE TABLE IF NOT EXISTS {_options.Keyspace}.{_options.TableName} (
                    id uuid PRIMARY KEY,
                    data text,
                    model_type text,
                    updated_at timestamp
                )";

            await session.ExecuteAsync(new SimpleStatement(createTable));

            _logger.LogInformation(
                "ScyllaDB schema initialized: {Keyspace}.{Table}",
                _options.Keyspace,
                _options.TableName);
        }
    }
}
