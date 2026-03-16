using System;
using System.Collections.Generic;

namespace DigiTFactory.Libraries.AbstractAggregate.Metadata
{
    /// <summary>
    /// Полные метаданные агрегата — весь Aggregate Design Canvas, загруженный из хранилища.
    /// </summary>
    public sealed class AggregateMetadata
    {
        /// <summary>
        /// Уникальный идентификатор записи метаданных.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя агрегата (Canvas §1 — Name).
        /// </summary>
        public string AggregateName { get; set; } = string.Empty;

        /// <summary>
        /// Описание назначения и границ агрегата (Canvas §2 — Description).
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Value Object-ы агрегата (Canvas §2 — структура данных).
        /// </summary>
        public IReadOnlyList<ValueObjectMetadata> ValueObjects { get; set; } = [];

        /// <summary>
        /// Бизнес-операции агрегата (Canvas §6 — Handled Commands).
        /// </summary>
        public IReadOnlyList<OperationMetadata> Operations { get; set; } = [];

        /// <summary>
        /// Переходы состояний (Canvas §3 — State Transitions).
        /// </summary>
        public IReadOnlyList<StateTransitionMetadata> StateTransitions { get; set; } = [];

        /// <summary>
        /// Подсказки по нагрузке (Canvas §8-9 — Throughput / Size).
        /// </summary>
        public ThroughputHints? Throughput { get; set; }
    }
}
