namespace DigiTFactory.Libraries.AbstractAggregate.Metadata
{
    /// <summary>
    /// Подсказки по нагрузке (секции 8-9 Aggregate Design Canvas — Throughput и Size).
    /// </summary>
    public sealed class ThroughputHints
    {
        /// <summary>
        /// Ожидаемое количество экземпляров агрегата.
        /// </summary>
        public int ExpectedInstanceCount { get; set; }

        /// <summary>
        /// Ожидаемое число операций в секунду.
        /// </summary>
        public int ExpectedOperationsPerSecond { get; set; }

        /// <summary>
        /// Среднее количество событий на экземпляр агрегата.
        /// </summary>
        public int AverageEventsPerInstance { get; set; }
    }
}
