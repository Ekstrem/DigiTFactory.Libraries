using System.Threading.Tasks;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Events;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.EventBus.Kafka
{
    /// <summary>
    /// Kafka реализация IEventBus.
    /// Композиция KafkaEventBusProducer и KafkaEventBusConsumer.
    /// </summary>
    public sealed class KafkaEventBus : IEventBus
    {
        private readonly KafkaEventBusProducer _producer;
        private readonly KafkaEventBusConsumer _consumer;

        internal KafkaEventBus(
            KafkaEventBusProducer producer,
            KafkaEventBusConsumer consumer)
        {
            _producer = producer;
            _consumer = consumer;
        }

        public void Publish<TBoundedContext>(IDomainEvent<TBoundedContext> domainEvent)
            where TBoundedContext : IBoundedContext
            => _producer.Publish(domainEvent);

        public Task PublishAsync<TBoundedContext>(IDomainEvent<TBoundedContext> domainEvent)
            where TBoundedContext : IBoundedContext
            => _producer.PublishAsync(domainEvent);

        public void Subscribe<TBoundedContext>(IDomainEventHandler<TBoundedContext> handler)
            where TBoundedContext : IBoundedContext
            => _consumer.Subscribe(handler);

        public void Unsubscribe<TBoundedContext>(IDomainEventHandler<TBoundedContext> handler)
            where TBoundedContext : IBoundedContext
            => _consumer.Unsubscribe(handler);
    }
}
