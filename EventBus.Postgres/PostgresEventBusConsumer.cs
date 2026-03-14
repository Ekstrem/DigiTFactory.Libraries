#nullable enable
using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.EventBus.Postgres.Configuration;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace DigiTFactory.Libraries.EventBus.Postgres
{
    /// <summary>
    /// Consumer: polling outbox таблицы и dispatch хендлерам.
    /// Реализован как IHostedService.
    /// </summary>
    internal sealed class PostgresEventBusConsumer : IHostedService, IDisposable
    {
        private readonly ConcurrentDictionary<Type, ImmutableList<object>> _handlers = new();
        private readonly PostgresEventBusOptions _options;
        private readonly PostgresOutboxInitializer _initializer;
        private readonly ILogger<PostgresEventBusConsumer> _logger;
        private CancellationTokenSource? _cts;
        private Task? _pollingTask;

        public PostgresEventBusConsumer(
            PostgresEventBusOptions options,
            PostgresOutboxInitializer initializer,
            ILogger<PostgresEventBusConsumer> logger)
        {
            _options = options;
            _initializer = initializer;
            _logger = logger;
        }

        public void Subscribe<TBoundedContext>(IDomainEventHandler<TBoundedContext> handler)
            where TBoundedContext : IBoundedContext
        {
            _handlers.AddOrUpdate(
                typeof(TBoundedContext),
                _ => ImmutableList.Create<object>(handler),
                (_, list) => list.Add(handler));

            _logger.LogDebug(
                "Subscribed handler {HandlerType} for {BoundedContext}",
                handler.GetType().Name, typeof(TBoundedContext).Name);
        }

        public void Unsubscribe<TBoundedContext>(IDomainEventHandler<TBoundedContext> handler)
            where TBoundedContext : IBoundedContext
        {
            _handlers.AddOrUpdate(
                typeof(TBoundedContext),
                _ => ImmutableList<object>.Empty,
                (_, list) => list.Remove(handler));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _initializer.InitializeAsync(cancellationToken);

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _pollingTask = Task.Factory.StartNew(
                () => PollLoop(_cts.Token),
                _cts.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);

            _logger.LogInformation("Postgres outbox consumer started, polling every {Interval}",
                _options.PollingInterval);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cts?.Cancel();

            if (_pollingTask != null)
            {
                await Task.WhenAny(_pollingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }

            _logger.LogInformation("Postgres outbox consumer stopped");
        }

        private async Task PollLoop(CancellationToken ct)
        {
            var schema = _options.SchemaName;
            var table = _options.TableName;

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var processed = await PollBatch(schema, table, ct);

                    if (processed == 0)
                    {
                        await Task.Delay(_options.PollingInterval, ct);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in outbox polling loop");
                    await Task.Delay(_options.PollingInterval, ct);
                }
            }
        }

        private async Task<int> PollBatch(string schema, string table, CancellationToken ct)
        {
            var selectSql = $@"
SELECT id, bounded_context, aggregate_id, version, correlation_token,
       command_name, subject_name, command_version,
       changed_value_objects::text, result, reason, created_at
FROM {schema}.{table}
WHERE processed_at IS NULL
ORDER BY created_at
LIMIT {_options.BatchSize}
FOR UPDATE SKIP LOCKED";

            var updateSql = $@"
UPDATE {schema}.{table}
SET processed_at = NOW()
WHERE id = @id";

            var count = 0;

            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await conn.OpenAsync(ct);

            await using var tx = await conn.BeginTransactionAsync(ct);

            await using (var selectCmd = new NpgsqlCommand(selectSql, conn, tx))
            await using (var reader = await selectCmd.ExecuteReaderAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                {
                    var envelope = new DomainEventEnvelope
                    {
                        AggregateId = reader.GetGuid(2),
                        Version = reader.GetInt64(3),
                        CorrelationToken = reader.GetGuid(4),
                        BoundedContext = reader.GetString(1),
                        CommandName = reader.GetString(5),
                        SubjectName = reader.GetString(6),
                        CommandVersion = reader.GetInt64(7),
                        ChangedValueObjectsJson = reader.GetString(8),
                        Result = reader.GetString(9),
                        Reason = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        CreatedAt = reader.GetDateTime(11)
                    };

                    var rowId = reader.GetGuid(0);
                    DispatchToHandlers(envelope);

                    await using var updateCmd = new NpgsqlCommand(updateSql, conn, tx);
                    updateCmd.Parameters.AddWithValue("id", rowId);
                    await updateCmd.ExecuteNonQueryAsync(ct);

                    count++;
                }
            }

            await tx.CommitAsync(ct);
            return count;
        }

        private void DispatchToHandlers(DomainEventEnvelope envelope)
        {
            foreach (var kvp in _handlers)
            {
                var boundedContextType = kvp.Key;
                if (!string.Equals(boundedContextType.Name, envelope.BoundedContext, StringComparison.OrdinalIgnoreCase))
                    continue;

                foreach (var handler in kvp.Value)
                {
                    try
                    {
                        if (handler is IObserver<DomainEventEnvelope> observer)
                        {
                            observer.OnNext(envelope);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex,
                            "Error dispatching outbox event to handler {HandlerType}",
                            handler.GetType().Name);
                    }
                }
            }
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
