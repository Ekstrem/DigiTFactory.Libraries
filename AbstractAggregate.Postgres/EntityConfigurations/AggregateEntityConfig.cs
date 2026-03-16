using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigiTFactory.Libraries.AbstractAggregate.Postgres.EntityConfigurations
{
    public class AggregateEntityConfig : IEntityTypeConfiguration<AggregateEntity>
    {
        public void Configure(EntityTypeBuilder<AggregateEntity> builder)
        {
            builder.ToTable("aggregates");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.AggregateName).HasColumnName("aggregate_name").HasMaxLength(256).IsRequired();
            builder.Property(e => e.Description).HasColumnName("description");
            builder.Property(e => e.ThroughputHintsJson).HasColumnName("throughput_hints").HasColumnType("jsonb");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.HasIndex(e => e.AggregateName).IsUnique();
        }
    }

    public class ValueObjectEntityConfig : IEntityTypeConfiguration<ValueObjectEntity>
    {
        public void Configure(EntityTypeBuilder<ValueObjectEntity> builder)
        {
            builder.ToTable("value_objects");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.AggregateId).HasColumnName("aggregate_id");
            builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(256).IsRequired();
            builder.Property(e => e.IsAggregateRoot).HasColumnName("is_aggregate_root");
            builder.Property(e => e.IsCollection).HasColumnName("is_collection");
            builder.HasOne(e => e.Aggregate).WithMany(a => a.ValueObjects).HasForeignKey(e => e.AggregateId);
            builder.HasIndex(e => new { e.AggregateId, e.Name }).IsUnique();
        }
    }

    public class ValueObjectPropertyEntityConfig : IEntityTypeConfiguration<ValueObjectPropertyEntity>
    {
        public void Configure(EntityTypeBuilder<ValueObjectPropertyEntity> builder)
        {
            builder.ToTable("value_object_properties");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.ValueObjectId).HasColumnName("value_object_id");
            builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(256).IsRequired();
            builder.Property(e => e.TypeName).HasColumnName("type_name").HasMaxLength(128).IsRequired();
            builder.Property(e => e.IsRequired).HasColumnName("is_required");
            builder.Property(e => e.DefaultValue).HasColumnName("default_value").HasColumnType("jsonb");
            builder.HasOne(e => e.ValueObject).WithMany(v => v.Properties).HasForeignKey(e => e.ValueObjectId);
            builder.HasIndex(e => new { e.ValueObjectId, e.Name }).IsUnique();
        }
    }

    public class OperationEntityConfig : IEntityTypeConfiguration<OperationEntity>
    {
        public void Configure(EntityTypeBuilder<OperationEntity> builder)
        {
            builder.ToTable("operations");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.AggregateId).HasColumnName("aggregate_id");
            builder.Property(e => e.CommandName).HasColumnName("command_name").HasMaxLength(256).IsRequired();
            builder.Property(e => e.Description).HasColumnName("description");
            builder.Property(e => e.MergeStrategy).HasColumnName("merge_strategy").HasMaxLength(64).IsRequired();
            builder.HasOne(e => e.Aggregate).WithMany(a => a.Operations).HasForeignKey(e => e.AggregateId);
            builder.HasIndex(e => new { e.AggregateId, e.CommandName }).IsUnique();
        }
    }

    public class OperationAffectedVoEntityConfig : IEntityTypeConfiguration<OperationAffectedVoEntity>
    {
        public void Configure(EntityTypeBuilder<OperationAffectedVoEntity> builder)
        {
            builder.ToTable("operation_affected_vos");
            builder.HasKey(e => new { e.OperationId, e.ValueObjectName });
            builder.Property(e => e.OperationId).HasColumnName("operation_id");
            builder.Property(e => e.ValueObjectName).HasColumnName("value_object_name").HasMaxLength(256);
            builder.HasOne(e => e.Operation).WithMany(o => o.AffectedVos).HasForeignKey(e => e.OperationId);
        }
    }

    public class InvariantEntityConfig : IEntityTypeConfiguration<InvariantEntity>
    {
        public void Configure(EntityTypeBuilder<InvariantEntity> builder)
        {
            builder.ToTable("invariants");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.OperationId).HasColumnName("operation_id");
            builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(256).IsRequired();
            builder.Property(e => e.InvariantType).HasColumnName("invariant_type").HasMaxLength(32).IsRequired();
            builder.Property(e => e.RuleExpression).HasColumnName("rule_expression").IsRequired();
            builder.Property(e => e.FailureReason).HasColumnName("failure_reason").IsRequired();
            builder.Property(e => e.FailureResult).HasColumnName("failure_result");
            builder.HasOne(e => e.Operation).WithMany(o => o.Invariants).HasForeignKey(e => e.OperationId);
        }
    }

    public class StateTransitionEntityConfig : IEntityTypeConfiguration<StateTransitionEntity>
    {
        public void Configure(EntityTypeBuilder<StateTransitionEntity> builder)
        {
            builder.ToTable("state_transitions");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.AggregateId).HasColumnName("aggregate_id");
            builder.Property(e => e.FromState).HasColumnName("from_state").HasMaxLength(256).IsRequired();
            builder.Property(e => e.ToState).HasColumnName("to_state").HasMaxLength(256).IsRequired();
            builder.Property(e => e.TriggerOperation).HasColumnName("trigger_operation").HasMaxLength(256).IsRequired();
            builder.HasOne(e => e.Aggregate).WithMany(a => a.StateTransitions).HasForeignKey(e => e.AggregateId);
        }
    }
}
