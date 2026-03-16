using DigiTFactory.Libraries.AbstractAggregate.Postgres.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace DigiTFactory.Libraries.AbstractAggregate.Postgres
{
    /// <summary>
    /// EF Core DbContext для хранения метаданных агрегатов.
    /// Схема: abstract_aggregate.
    /// </summary>
    public class MetadataDbContext : DbContext
    {
        public MetadataDbContext(DbContextOptions<MetadataDbContext> options)
            : base(options)
        {
        }

        public DbSet<AggregateEntity> Aggregates { get; set; } = null!;
        public DbSet<ValueObjectEntity> ValueObjects { get; set; } = null!;
        public DbSet<ValueObjectPropertyEntity> ValueObjectProperties { get; set; } = null!;
        public DbSet<OperationEntity> Operations { get; set; } = null!;
        public DbSet<OperationAffectedVoEntity> OperationAffectedVos { get; set; } = null!;
        public DbSet<InvariantEntity> Invariants { get; set; } = null!;
        public DbSet<StateTransitionEntity> StateTransitions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("abstract_aggregate");

            modelBuilder.ApplyConfiguration(new AggregateEntityConfig());
            modelBuilder.ApplyConfiguration(new ValueObjectEntityConfig());
            modelBuilder.ApplyConfiguration(new ValueObjectPropertyEntityConfig());
            modelBuilder.ApplyConfiguration(new OperationEntityConfig());
            modelBuilder.ApplyConfiguration(new OperationAffectedVoEntityConfig());
            modelBuilder.ApplyConfiguration(new InvariantEntityConfig());
            modelBuilder.ApplyConfiguration(new StateTransitionEntityConfig());
        }
    }
}
