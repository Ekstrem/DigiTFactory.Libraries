using System.Threading.Tasks;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Events;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.EventBus.Postgres
{
    /// <summary>
    /// PostgreSQL (outbox) реализация IEventBus.
    /// Композиция PostgresEventBusProducer и PostgresEventBusConsumer.
    /// </summary>
    public sealed class PostgresEventBus : IEventBus
    {
        private readonly PostgresEventBusProducer _producer;
        private readonly PostgresEventBusConsumer _consumer;

        internal PostgresEventBus(
            PostgresEventBusProducer producer,
            PostgresEventBusConsumer consumer)
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
