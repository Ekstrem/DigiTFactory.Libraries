using DigiTFactory.Libraries.CommandRepository.Postgres.Configuration;
using DigiTFactory.Libraries.CommandRepository.Postgres.Entities;
using DigiTFactory.Libraries.CommandRepository.Postgres.Mapping;
using Microsoft.EntityFrameworkCore;

namespace DigiTFactory.Libraries.CommandRepository.Postgres
{
    /// <summary>
    /// DbContext для Event Store. Содержит таблицы событий, снапшотов и состояний агрегатов.
    /// Микросервис может наследоваться от этого контекста для добавления своих Read Models.
    /// </summary>
    public class EventStoreDbContext : DbContext
    {
        private readonly EventStoreOptions _options;

        public EventStoreDbContext(
            DbContextOptions options,
            EventStoreOptions storeOptions)
            : base(options)
        {
            _options = storeOptions;
        }

        /// <summary>Таблица доменных событий.</summary>
        public DbSet<DomainEventEntry> Events { get; set; }

        /// <summary>Таблица снапшотов агрегатов (стратегия SnapshotAfterN).</summary>
        public DbSet<SnapshotEntry> Snapshots { get; set; }

        /// <summary>Таблица текущих состояний агрегатов (стратегия StateOnly).</summary>
        public DbSet<AggregateStateEntry> AggregateStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_options.SchemaName);
            modelBuilder.ApplyConfiguration(new DomainEventEntryMapping());
            modelBuilder.ApplyConfiguration(new SnapshotEntryMapping());
            modelBuilder.ApplyConfiguration(new AggregateStateEntryMapping());
        }
    }
}
