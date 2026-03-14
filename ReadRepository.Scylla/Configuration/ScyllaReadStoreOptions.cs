namespace DigiTFactory.Libraries.ReadRepository.Scylla.Configuration
{
    /// <summary>
    /// Настройки ScyllaDB Read Store.
    /// </summary>
    public class ScyllaReadStoreOptions
    {
        /// <summary>
        /// Точки подключения к ScyllaDB/Cassandra.
        /// </summary>
        public string[] ContactPoints { get; set; } = new[] { "localhost" };

        /// <summary>
        /// Порт ScyllaDB/Cassandra.
        /// </summary>
        public int Port { get; set; } = 9042;

        /// <summary>
        /// Имя keyspace.
        /// </summary>
        public string Keyspace { get; set; } = "read_models";

        /// <summary>
        /// Имя таблицы для Read-моделей.
        /// </summary>
        public string TableName { get; set; } = "projections";

        /// <summary>
        /// Стратегия репликации (SimpleStrategy или NetworkTopologyStrategy).
        /// </summary>
        public string ReplicationStrategy { get; set; } = "SimpleStrategy";

        /// <summary>
        /// Фактор репликации.
        /// </summary>
        public int ReplicationFactor { get; set; } = 1;

        /// <summary>
        /// Автоматически создавать keyspace и таблицу при старте.
        /// </summary>
        public bool AutoCreateSchema { get; set; } = true;
    }
}
