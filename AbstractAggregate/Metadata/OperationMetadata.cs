using System.Collections.Generic;

namespace DigiTFactory.Libraries.AbstractAggregate.Metadata
{
    /// <summary>
    /// Метаданные бизнес-операции агрегата (секция 6 — Handled Commands, секция 7 — Created Events).
    /// </summary>
    public sealed class OperationMetadata
    {
        /// <summary>
        /// Имя команды (например "SubscriberRequestQuestion", "OperatorRepliedToMessage").
        /// </summary>
        public string CommandName { get; set; } = string.Empty;

        /// <summary>
        /// Описание операции.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Имена порождаемых доменных событий (Canvas §7 — Created Events).
        /// </summary>
        public IReadOnlyList<string> ProducedEvents { get; set; } = [];

        /// <summary>
        /// Инварианты, применяемые при выполнении операции (Canvas §4).
        /// </summary>
        public IReadOnlyList<InvariantMetadata> Invariants { get; set; } = [];

        /// <summary>
        /// Имена Value Object-ов, которые затрагивает эта операция.
        /// </summary>
        public IReadOnlyList<string> AffectedValueObjects { get; set; } = [];

        /// <summary>
        /// Стратегия слияния: как incoming VOs объединяются с текущим состоянием.
        /// </summary>
        public OperationMergeStrategy MergeStrategy { get; set; } = OperationMergeStrategy.ReplaceAffected;
    }
}
