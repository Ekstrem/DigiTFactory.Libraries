using DigiTFactory.Libraries.CommandRepository.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigiTFactory.Libraries.CommandRepository.Postgres.Mapping
{
    internal class SnapshotEntryMapping : IEntityTypeConfiguration<SnapshotEntry>
    {
        public void Configure(EntityTypeBuilder<SnapshotEntry> builder)
        {
            builder.ToTable("Snapshots");

            builder.HasKey(x => new { x.Id, x.Version });

            builder.Property(x => x.AggregateJson)
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasIndex(x => x.Id);
        }
    }
}
