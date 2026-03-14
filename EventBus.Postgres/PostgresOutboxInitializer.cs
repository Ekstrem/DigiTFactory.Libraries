using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.EventBus.Postgres.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace DigiTFactory.Libraries.EventBus.Postgres
{
    /// <summary>
    /// Создание таблицы outbox при старте (если AutoCreateTable = true).
    /// </summary>
    internal sealed class PostgresOutboxInitializer
    {
        private readonly PostgresEventBusOptions _options;
        private readonly ILogger<PostgresOutboxInitializer> _logger;

        public PostgresOutboxInitializer(
            PostgresEventBusOptions options,
            ILogger<PostgresOutboxInitializer> logger)
        {
            _options = options;
            _logger = logger;
        }

        public async Task InitializeAsync(CancellationToken ct = default)
        {
            if (!_options.AutoCreateTable)
                return;

            var schema = _options.SchemaName;
            var table = _options.TableName;

            var sql = $@"
CREATE TABLE IF NOT EXISTS {schema}.{table} (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    bounded_context TEXT NOT NULL,
    aggregate_id UUID NOT NULL,
    version BIGINT NOT NULL,
    correlation_token UUID NOT NULL,
    command_name TEXT NOT NULL,
    subject_name TEXT NOT NULL,
    command_version BIGINT NOT NULL,
    changed_value_objects JSONB NOT NULL DEFAULT '{{}}',
    result TEXT NOT NULL,
    reason TEXT,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    processed_at TIMESTAMPTZ NULL
);
CREATE INDEX IF NOT EXISTS ix_{table}_unprocessed
    ON {schema}.{table} (created_at) WHERE processed_at IS NULL;
";

            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await conn.OpenAsync(ct);

            await using var cmd = new NpgsqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync(ct);

            _logger.LogInformation("Outbox table {Schema}.{Table} initialized", schema, table);
        }
    }
}
