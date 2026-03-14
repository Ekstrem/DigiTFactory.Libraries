using System;

namespace DigiTFactory.Libraries.EventBus.Postgres.Configuration
{
    /// <summary>
    /// Настройки PostgreSQL EventBus (outbox pattern).
    /// </summary>
    public class PostgresEventBusOptions
    {
        /// <summary>Строка подключения к PostgreSQL.</summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>Схема таблицы outbox.</summary>
        public string SchemaName { get; set; } = "public";

        /// <summary>Имя таблицы outbox.</summary>
        public string TableName { get; set; } = "domain_events_outbox";

        /// <summary>Интервал polling необработанных событий.</summary>
        public TimeSpan PollingInterval { get; set; } = TimeSpan.FromSeconds(5);

        /// <summary>Максимальное количество событий за один poll.</summary>
        public int BatchSize { get; set; } = 100;

        /// <summary>Автоматически создавать таблицу при старте.</summary>
        public bool AutoCreateTable { get; set; } = true;
    }
}
