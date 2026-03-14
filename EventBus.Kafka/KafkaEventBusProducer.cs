using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using DigiTFactory.Libraries.EventBus.Kafka.Configuration;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Events;
using Microsoft.Extensions.Logging;

namespace DigiTFactory.Libraries.EventBus.Kafka
{
    /// <summary>
    /// Kafka producer для доменных событий.
    /// </summary>
    internal sealed class KafkaEventBusProducer : IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaEventBusOptions _options;
        private readonly ILogger<KafkaEventBusProducer> _logger;

        public KafkaEventBusProducer(
            KafkaEventBusOptions options,
            ILogger<KafkaEventBusProducer> logger)
        {
            _options = options;
            _logger = logger;

            var config = new ProducerConfig
            {
                BootstrapServers = options.BootstrapServers
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public void Publish<TBoundedContext>(IDomainEvent<TBoundedContext> domainEvent)
            where TBoundedContext : IBoundedContext
        {
            PublishAsync(domainEvent).GetAwaiter().GetResult();
        }

        public async Task PublishAsync<TBoundedContext>(IDomainEvent<TBoundedContext> domainEvent)
            where TBoundedContext : IBoundedContext
        {
            var topic = GetTopicName<TBoundedContext>();
            var json = DomainEventSerializer.Serialize(domainEvent);
            var key = domainEvent.CorrelationToken.ToString();

            var message = new Message<string, string>
            {
                Key = key,
                Value = json
            };

            var result = await _producer.ProduceAsync(topic, message);

            _logger.LogDebug(
                "Published event to Kafka topic {Topic}, partition {Partition}, offset {Offset}",
                topic, result.Partition.Value, result.Offset.Value);
        }

        private string GetTopicName<TBoundedContext>()
            => $"{_options.TopicPrefix}.{typeof(TBoundedContext).Name.ToLowerInvariant()}";

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }
    }
}
