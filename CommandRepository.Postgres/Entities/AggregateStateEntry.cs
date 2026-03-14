using System;

namespace DigiTFactory.Libraries.CommandRepository.Postgres.Entities
{
    /// <summary>
    /// Текущее состояние агрегата целиком. Используется стратегией <see cref="Configuration.EventStoreStrategy.StateOnly"/>.
    /// Хранится в таблице AggregateStates, обновляется при каждой команде (UPSERT).
    /// </summary>
    public class AggregateStateEntry
    {
        /// <summary>Идентификатор агрегата.</summary>
        public Guid Id { get; set; }

        /// <summary>Текущая версия агрегата.</summary>
        public long Version { get; set; }

        /// <summary>Сериализованный агрегат (JSONB в PostgreSQL).</summary>
        public string AggregateJson { get; set; } = string.Empty;

        /// <summary>Дата последнего обновления (UTC).</summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
