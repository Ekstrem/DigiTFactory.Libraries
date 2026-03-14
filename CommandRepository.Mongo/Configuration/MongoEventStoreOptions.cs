namespace DigiTFactory.Libraries.CommandRepository.Mongo.Configuration
{
    /// <summary>
    /// Настройки MongoDB Event Store.
    /// </summary>
    public class MongoEventStoreOptions
    {
        /// <summary>Строка подключения к MongoDB.</summary>
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";

        /// <summary>Имя базы данных.</summary>
        public string DatabaseName { get; set; } = "EventStore";

        /// <summary>Стратегия хранения событий.</summary>
        public EventStoreStrategy Strategy { get; set; } = EventStoreStrategy.FullEventSourcing;

        /// <summary>Интервал создания snapshot (только для SnapshotAfterN).</summary>
        public int SnapshotInterval { get; set; } = 10;

        /// <summary>Префикс имён коллекций (например, "Chat_" → "Chat_DomainEvents").</summary>
        public string CollectionPrefix { get; set; } = "";
    }
}
