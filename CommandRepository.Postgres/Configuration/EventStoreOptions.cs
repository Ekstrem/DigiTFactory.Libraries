namespace DigiTFactory.Libraries.CommandRepository.Postgres.Configuration
{
    /// <summary>
    /// Настройки Event Store. Задаются при старте микросервиса через DI.
    /// </summary>
    public class EventStoreOptions
    {
        /// <summary>
        /// Стратегия хранения событий.
        /// </summary>
        public EventStoreStrategy Strategy { get; set; } = EventStoreStrategy.FullEventSourcing;

        /// <summary>
        /// Интервал создания snapshot (только для <see cref="EventStoreStrategy.SnapshotAfterN"/>).
        /// Snapshot создаётся каждые N событий.
        /// </summary>
        public int SnapshotInterval { get; set; } = 10;

        /// <summary>
        /// Имя схемы PostgreSQL для таблиц Event Store.
        /// </summary>
        public string SchemaName { get; set; } = "Commands";
    }
}
