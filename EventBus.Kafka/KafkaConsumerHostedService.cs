using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace DigiTFactory.Libraries.EventBus.Kafka
{
    /// <summary>
    /// Hosted service для управления lifecycle Kafka consumer loops.
    /// </summary>
    internal sealed class KafkaConsumerHostedService : IHostedService
    {
        private readonly KafkaEventBusConsumer _consumer;

        public KafkaConsumerHostedService(KafkaEventBusConsumer consumer)
        {
            _consumer = consumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.StartConsuming(cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Dispose();
            return Task.CompletedTask;
        }
    }
}
