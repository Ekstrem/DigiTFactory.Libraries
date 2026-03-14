using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using DigiTFactory.Libraries.EventBus.Kafka.Configuration;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Events;
using Microsoft.Extensions.Logging;

namespace DigiTFactory.Libraries.EventBus.Kafka
{
    /// <summary>
    /// Kafka consumer для доменных событий.
    /// Управляет подписками и background consumer tasks.
    /// </summary>
    internal sealed class KafkaEventBusConsumer : IDisposable
    {
        private readonly ConcurrentDictionary<Type, ImmutableList<object>> _handlers = new();
        private readonly ConcurrentDictionary<Type, CancellationTokenSource> _consumerTasks = new();
        private readonly KafkaEventBusOptions _options;
        private readonly ILogger<KafkaEventBusConsumer> _logger;

        public KafkaEventBusConsumer(
            KafkaEventBusOptions options,
            ILogger<KafkaEventBusConsumer> logger)
        {
            _options = options;
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

            _logger.LogDebug(
                "Unsubscribed handler {HandlerType} for {BoundedContext}",
                handler.GetType().Name, typeof(TBoundedContext).Name);
        }

        /// <summary>
        /// Запускает consumer loop для всех зарегистрированных bounded contexts.
        /// Вызывается из KafkaConsumerHostedService.
        /// </summary>
        public void StartConsuming(CancellationToken stoppingToken)
        {
            foreach (var kvp in _handlers)
            {
                var boundedContextType = kvp.Key;
                var topic = $"{_options.TopicPrefix}.{boundedContextType.Name.ToLowerInvariant()}";

                var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                _consumerTasks[boundedContextType] = cts;

                Task.Factory.StartNew(
                    () => ConsumeLoop(topic, boundedContextType, cts.Token),
                    cts.Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);

                _logger.LogInformation("Started Kafka consumer for topic {Topic}", topic);
            }
        }

        private void ConsumeLoop(string topic, Type boundedContextType, CancellationToken ct)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _options.BootstrapServers,
                GroupId = _options.GroupId,
                AutoOffsetReset = _options.AutoOffsetReset,
                EnableAutoCommit = false
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(topic);

            _logger.LogInformation("Kafka consumer subscribed to topic {Topic}", topic);

            try
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(ct);
                        if (result?.Message?.Value == null) continue;

                        var envelope = DomainEventSerializer.Deserialize(result.Message.Value);
                        DispatchToHandlers(boundedContextType, envelope);

                        consumer.Commit(result);
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(ex, "Kafka consume error on topic {Topic}", topic);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Штатное завершение
            }
            finally
            {
                consumer.Close();
            }
        }

        private void DispatchToHandlers(Type boundedContextType, DomainEventEnvelope envelope)
        {
            if (!_handlers.TryGetValue(boundedContextType, out var handlers))
                return;

            foreach (var handler in handlers)
            {
                try
                {
                    // Dispatch через IObserver<DomainEventEnvelope>
                    if (handler is IObserver<DomainEventEnvelope> observer)
                    {
                        observer.OnNext(envelope);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error dispatching Kafka event to handler {HandlerType}",
                        handler.GetType().Name);
                }
            }
        }

        public void Dispose()
        {
            foreach (var cts in _consumerTasks.Values)
            {
                cts.Cancel();
                cts.Dispose();
            }

            _consumerTasks.Clear();
        }
    }
}
