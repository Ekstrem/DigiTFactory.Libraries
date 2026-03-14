using Confluent.Kafka;

namespace DigiTFactory.Libraries.EventBus.Kafka.Configuration
{
    /// <summary>
    /// Настройки Kafka EventBus.
    /// </summary>
    public class KafkaEventBusOptions
    {
        /// <summary>Адреса Kafka брокеров.</summary>
        public string BootstrapServers { get; set; } = "localhost:9092";

        /// <summary>Префикс имён топиков (например, "domain-events" → "domain-events.ichat").</summary>
        public string TopicPrefix { get; set; } = "domain-events";

        /// <summary>Идентификатор consumer group.</summary>
        public string GroupId { get; set; } = "default-group";

        /// <summary>Позиция чтения при отсутствии offset.</summary>
        public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Earliest;
    }
}
