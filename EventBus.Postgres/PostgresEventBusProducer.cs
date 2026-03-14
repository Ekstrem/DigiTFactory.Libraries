using System.Threading.Tasks;
using DigiTFactory.Libraries.EventBus.Postgres.Configuration;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Events;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;

namespace DigiTFactory.Libraries.EventBus.Postgres
{
    /// <summary>
    /// Producer: INSERT доменных событий в outbox таблицу.
    /// </summary>
    internal sealed class PostgresEventBusProducer
    {
        private readonly PostgresEventBusOptions _options;
        private readonly ILogger<PostgresEventBusProducer> _logger;

        public PostgresEventBusProducer(
            PostgresEventBusOptions options,
            ILogger<PostgresEventBusProducer> logger)
        {
            _options = options;
            _logger = logger;
        }

        public void Publish<TBoundedContext>(IDomainEvent<TBoundedContext> domainEvent)
            where TBoundedContext : IBoundedContext
        {
            PublishAsync(domainEvent).GetAwaiter().GetResult();
        }

        public async Task PublishAsync<TBoundedContext>(IDomainEvent<TBoundedContext> domainEvent)
            where TBoundedContext : IBoundedContext
        {
            var envelope = DomainEventSerializer.ToEnvelope(domainEvent);
            var schema = _options.SchemaName;
            var table = _options.TableName;

            var sql = $@"
INSERT INTO {schema}.{table}
    (bounded_context, aggregate_id, version, correlation_token,
     command_name, subject_name, command_version,
     changed_value_objects, result, reason, created_at)
VALUES
    (@bounded_context, @aggregate_id, @version, @correlation_token,
     @command_name, @subject_name, @command_version,
     @changed_value_objects::jsonb, @result, @reason, @created_at)";

            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("bounded_context", envelope.BoundedContext);
            cmd.Parameters.AddWithValue("aggregate_id", envelope.AggregateId);
            cmd.Parameters.AddWithValue("version", envelope.Version);
            cmd.Parameters.AddWithValue("correlation_token", envelope.CorrelationToken);
            cmd.Parameters.AddWithValue("command_name", envelope.CommandName);
            cmd.Parameters.AddWithValue("subject_name", envelope.SubjectName);
            cmd.Parameters.AddWithValue("command_version", envelope.CommandVersion);
            cmd.Parameters.AddWithValue("changed_value_objects", NpgsqlDbType.Jsonb, envelope.ChangedValueObjectsJson);
            cmd.Parameters.AddWithValue("result", envelope.Result);
            cmd.Parameters.AddWithValue("reason", (object)envelope.Reason ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("created_at", envelope.CreatedAt);

            await cmd.ExecuteNonQueryAsync();

            _logger.LogDebug(
                "Published event to outbox {Schema}.{Table} for aggregate {AggregateId}",
                schema, table, envelope.AggregateId);
        }
    }
}
