namespace DigiTFactory.Libraries.CommandRepository.Mongo.Configuration
{
    /// <summary>
    /// Стратегия хранения событий в Event Store.
    /// </summary>
    public enum EventStoreStrategy
    {
        /// <summary>
        /// Все события сохраняются, агрегат восстанавливается из полного стрима.
        /// </summary>
        FullEventSourcing = 0,

        /// <summary>
        /// Каждые N событий сохраняется snapshot агрегата в отдельную коллекцию.
        /// При чтении: snapshot + события после него.
        /// </summary>
        SnapshotAfterN = 1,

        /// <summary>
        /// Каждый раз сохраняется агрегат целиком (без истории событий).
        /// </summary>
        StateOnly = 2
    }
}
