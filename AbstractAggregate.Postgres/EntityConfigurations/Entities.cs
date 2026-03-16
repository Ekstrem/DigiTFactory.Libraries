using System;
using System.Collections.Generic;

namespace DigiTFactory.Libraries.AbstractAggregate.Postgres.EntityConfigurations
{
    /// <summary>
    /// EF Core entity: агрегат.
    /// </summary>
    public class AggregateEntity
    {
        public Guid Id { get; set; }
        public string AggregateName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ThroughputHintsJson { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ValueObjectEntity> ValueObjects { get; set; } = new List<ValueObjectEntity>();
        public ICollection<OperationEntity> Operations { get; set; } = new List<OperationEntity>();
        public ICollection<StateTransitionEntity> StateTransitions { get; set; } = new List<StateTransitionEntity>();
    }

    /// <summary>
    /// EF Core entity: Value Object.
    /// </summary>
    public class ValueObjectEntity
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsAggregateRoot { get; set; }
        public bool IsCollection { get; set; }

        public AggregateEntity Aggregate { get; set; } = null!;
        public ICollection<ValueObjectPropertyEntity> Properties { get; set; } = new List<ValueObjectPropertyEntity>();
    }

    /// <summary>
    /// EF Core entity: свойство Value Object.
    /// </summary>
    public class ValueObjectPropertyEntity
    {
        public Guid Id { get; set; }
        public Guid ValueObjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TypeName { get; set; } = "string";
        public bool IsRequired { get; set; }
        public string? DefaultValue { get; set; }

        public ValueObjectEntity ValueObject { get; set; } = null!;
    }

    /// <summary>
    /// EF Core entity: бизнес-операция.
    /// </summary>
    public class OperationEntity
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public string CommandName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string MergeStrategy { get; set; } = "ReplaceAffected";

        public AggregateEntity Aggregate { get; set; } = null!;
        public ICollection<OperationAffectedVoEntity> AffectedVos { get; set; } = new List<OperationAffectedVoEntity>();
        public ICollection<InvariantEntity> Invariants { get; set; } = new List<InvariantEntity>();
    }

    /// <summary>
    /// EF Core entity: затронутый VO операцией.
    /// </summary>
    public class OperationAffectedVoEntity
    {
        public Guid OperationId { get; set; }
        public string ValueObjectName { get; set; } = string.Empty;

        public OperationEntity Operation { get; set; } = null!;
    }

    /// <summary>
    /// EF Core entity: инвариант.
    /// </summary>
    public class InvariantEntity
    {
        public Guid Id { get; set; }
        public Guid OperationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string InvariantType { get; set; } = "Assertion";
        public string RuleExpression { get; set; } = string.Empty;
        public string FailureReason { get; set; } = string.Empty;
        public int FailureResult { get; set; } = 2; // DomainOperationResultEnum.Exception

        public OperationEntity Operation { get; set; } = null!;
    }

    /// <summary>
    /// EF Core entity: переход состояний.
    /// </summary>
    public class StateTransitionEntity
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public string FromState { get; set; } = string.Empty;
        public string ToState { get; set; } = string.Empty;
        public string TriggerOperation { get; set; } = string.Empty;

        public AggregateEntity Aggregate { get; set; } = null!;
    }
}
