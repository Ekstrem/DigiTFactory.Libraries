using DigiTFactory.Libraries.CommandRepository.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigiTFactory.Libraries.CommandRepository.Postgres.Mapping
{
    internal class AggregateStateEntryMapping : IEntityTypeConfiguration<AggregateStateEntry>
    {
        public void Configure(EntityTypeBuilder<AggregateStateEntry> builder)
        {
            builder.ToTable("AggregateStates");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Version)
                .IsRequired();

            builder.Property(x => x.AggregateJson)
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();
        }
    }
}
