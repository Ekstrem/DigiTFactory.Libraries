using DigiTFactory.Libraries.SeedWorks.Result;

namespace DigiTFactory.Libraries.AbstractAggregate.Metadata
{
    /// <summary>
    /// Метаданные инварианта бизнес-операции (секция 4 Aggregate Design Canvas — Enforced Invariants).
    /// </summary>
    public sealed class InvariantMetadata
    {
        /// <summary>
        /// Имя инварианта.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Тип инварианта: Assertion (блокирующий) или Validator (предупреждение).
        /// </summary>
        public InvariantType Type { get; set; } = InvariantType.Assertion;

        /// <summary>
        /// Выражение правила на DSL (например "IsNewEntity", "PropertyNotNull:Root.UserId").
        /// </summary>
        public string RuleExpression { get; set; } = string.Empty;

        /// <summary>
        /// Причина при нарушении инварианта.
        /// </summary>
        public string FailureReason { get; set; } = string.Empty;

        /// <summary>
        /// Результат операции при нарушении (Exception или WithWarnings).
        /// </summary>
        public DomainOperationResultEnum FailureResult { get; set; } = DomainOperationResultEnum.Exception;
    }
}
