using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.AbstractAggregate.Metadata;
using DigiTFactory.Libraries.AbstractAggregate.Postgres.EntityConfigurations;
using DigiTFactory.Libraries.AbstractAggregate.Repository;
using DigiTFactory.Libraries.SeedWorks.Result;
using Microsoft.EntityFrameworkCore;

namespace DigiTFactory.Libraries.AbstractAggregate.Postgres.Repositories
{
    /// <summary>
    /// PostgreSQL реализация IMetadataRepository.
    /// Загружает и сохраняет метаданные агрегатов через EF Core.
    /// </summary>
    public sealed class PostgresMetadataRepository : IMetadataRepository
    {
        private readonly MetadataDbContext _context;

        public PostgresMetadataRepository(MetadataDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public async Task<AggregateMetadata?> GetByNameAsync(string aggregateName, CancellationToken ct = default)
        {
            var entity = await LoadAggregateEntity(
                q => q.Where(a => a.AggregateName == aggregateName), ct);
            return entity != null ? MapToMetadata(entity) : null;
        }

        /// <inheritdoc />
        public async Task<AggregateMetadata?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await LoadAggregateEntity(
                q => q.Where(a => a.Id == id), ct);
            return entity != null ? MapToMetadata(entity) : null;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<AggregateMetadata>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await IncludeAll(_context.Aggregates).ToListAsync(ct);
            return entities.Select(MapToMetadata).ToList();
        }

        /// <inheritdoc />
        public async Task SaveAsync(AggregateMetadata metadata, CancellationToken ct = default)
        {
            var existing = await _context.Aggregates
                .FirstOrDefaultAsync(a => a.Id == metadata.Id, ct);

            if (existing != null)
            {
                existing.AggregateName = metadata.AggregateName;
                existing.Description = metadata.Description;
                existing.ThroughputHintsJson = metadata.Throughput != null
                    ? JsonSerializer.Serialize(metadata.Throughput)
                    : null;
                existing.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                var entity = new AggregateEntity
                {
                    Id = metadata.Id == Guid.Empty ? Guid.NewGuid() : metadata.Id,
                    AggregateName = metadata.AggregateName,
                    Description = metadata.Description,
                    ThroughputHintsJson = metadata.Throughput != null
                        ? JsonSerializer.Serialize(metadata.Throughput)
                        : null
                };
                _context.Aggregates.Add(entity);
            }

            await _context.SaveChangesAsync(ct);
        }

        private async Task<AggregateEntity?> LoadAggregateEntity(
            Func<IQueryable<AggregateEntity>, IQueryable<AggregateEntity>> filter,
            CancellationToken ct)
        {
            return await IncludeAll(filter(_context.Aggregates)).FirstOrDefaultAsync(ct);
        }

        private static IQueryable<AggregateEntity> IncludeAll(IQueryable<AggregateEntity> query)
        {
            return query
                .Include(a => a.ValueObjects)
                    .ThenInclude(vo => vo.Properties)
                .Include(a => a.Operations)
                    .ThenInclude(op => op.AffectedVos)
                .Include(a => a.Operations)
                    .ThenInclude(op => op.Invariants)
                .Include(a => a.StateTransitions);
        }

        private static AggregateMetadata MapToMetadata(AggregateEntity entity)
        {
            return new AggregateMetadata
            {
                Id = entity.Id,
                AggregateName = entity.AggregateName,
                Description = entity.Description,
                Throughput = !string.IsNullOrEmpty(entity.ThroughputHintsJson)
                    ? JsonSerializer.Deserialize<ThroughputHints>(entity.ThroughputHintsJson)
                    : null,
                ValueObjects = entity.ValueObjects.Select(MapValueObject).ToList(),
                Operations = entity.Operations.Select(MapOperation).ToList(),
                StateTransitions = entity.StateTransitions.Select(MapStateTransition).ToList()
            };
        }

        private static ValueObjectMetadata MapValueObject(ValueObjectEntity entity)
        {
            return new ValueObjectMetadata
            {
                Name = entity.Name,
                IsAggregateRoot = entity.IsAggregateRoot,
                IsCollection = entity.IsCollection,
                Properties = entity.Properties.Select(p => new PropertyMetadata
                {
                    Name = p.Name,
                    TypeName = p.TypeName,
                    IsRequired = p.IsRequired,
                    DefaultValue = p.DefaultValue
                }).ToList()
            };
        }

        private static OperationMetadata MapOperation(OperationEntity entity)
        {
            return new OperationMetadata
            {
                CommandName = entity.CommandName,
                Description = entity.Description,
                MergeStrategy = Enum.TryParse<OperationMergeStrategy>(entity.MergeStrategy, true, out var ms)
                    ? ms
                    : OperationMergeStrategy.ReplaceAffected,
                AffectedValueObjects = entity.AffectedVos.Select(a => a.ValueObjectName).ToList(),
                Invariants = entity.Invariants.Select(MapInvariant).ToList(),
                ProducedEvents = [] // Events are metadata-only, not stored separately
            };
        }

        private static InvariantMetadata MapInvariant(InvariantEntity entity)
        {
            return new InvariantMetadata
            {
                Name = entity.Name,
                Type = Enum.TryParse<InvariantType>(entity.InvariantType, true, out var it)
                    ? it
                    : InvariantType.Assertion,
                RuleExpression = entity.RuleExpression,
                FailureReason = entity.FailureReason,
                FailureResult = (DomainOperationResultEnum)entity.FailureResult
            };
        }

        private static StateTransitionMetadata MapStateTransition(StateTransitionEntity entity)
        {
            return new StateTransitionMetadata
            {
                FromState = entity.FromState,
                ToState = entity.ToState,
                TriggerOperation = entity.TriggerOperation
            };
        }
    }
}
