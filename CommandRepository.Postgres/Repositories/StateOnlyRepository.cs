using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.CommandRepository.Postgres.Entities;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DigiTFactory.Libraries.CommandRepository.Postgres.Repositories
{
    /// <summary>
    /// Стратегия 3: State Only.
    /// Каждый раз сохраняется агрегат целиком (UPSERT), без истории событий.
    /// </summary>
    internal sealed class StateOnlyRepository<TBoundedContext, TAnemicModel>
        : IEventStoreRepository<TBoundedContext, TAnemicModel>
        where TBoundedContext : IBoundedContext
        where TAnemicModel : IAnemicModel<TBoundedContext>
    {
        private readonly EventStoreDbContext _dbContext;
        private readonly ILogger _logger;

        public StateOnlyRepository(
            EventStoreDbContext dbContext,
            ILogger<StateOnlyRepository<TBoundedContext, TAnemicModel>> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<TAnemicModel>> GetById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogDebug("StateOnly: Getting aggregate state {AggregateId}", id);

            var state = await _dbContext.AggregateStates
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            if (state == null)
            {
                _logger.LogDebug("StateOnly: No state found for aggregate {AggregateId}", id);
                return new List<TAnemicModel>();
            }

            // TODO: Десериализовать AggregateJson в TAnemicModel
            return new List<TAnemicModel>();
        }

        public async Task<TAnemicModel> GetByIdAndVersion(Guid id, long version, CancellationToken cancellationToken)
        {
            _logger.LogDebug("StateOnly: Getting aggregate {AggregateId} version {Version}", id, version);

            var state = await _dbContext.AggregateStates
                .FirstOrDefaultAsync(s => s.Id == id && s.Version == version, cancellationToken);

            // TODO: Десериализовать AggregateJson в TAnemicModel
            return default!;
        }

        public async Task<TAnemicModel> GetByCorrelationToken(Guid correlationToken, CancellationToken cancellationToken)
        {
            _logger.LogDebug("StateOnly: GetByCorrelationToken is not optimal for StateOnly strategy");

            // В StateOnly нет таблицы событий с CorrelationToken.
            // Fallback: ищем в Events если они есть, иначе возвращаем default.
            var entry = await _dbContext.Events
                .FirstOrDefaultAsync(e => e.CorrelationToken == correlationToken, cancellationToken);

            if (entry == null)
                return default!;

            // Найден ID агрегата через событие — загружаем состояние
            return await GetByIdAndVersion(entry.Id, entry.Version, cancellationToken);
        }

        public async Task SaveEventAsync(DomainEventEntry entry, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "StateOnly: Saving aggregate state for {AggregateId}, version {Version}, command {CommandName}",
                entry.Id, entry.Version, entry.CommandName);

            var existingState = await _dbContext.AggregateStates
                .FirstOrDefaultAsync(s => s.Id == entry.Id, cancellationToken);

            if (existingState == null)
            {
                var newState = new AggregateStateEntry
                {
                    Id = entry.Id,
                    Version = entry.Version,
                    AggregateJson = entry.ChangedValueObjectsJson,
                    UpdatedAt = DateTime.UtcNow
                };
                _dbContext.AggregateStates.Add(newState);
            }
            else
            {
                existingState.Version = entry.Version;
                existingState.AggregateJson = entry.ChangedValueObjectsJson;
                existingState.UpdatedAt = DateTime.UtcNow;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogDebug("StateOnly: Aggregate state saved for {AggregateId}", entry.Id);
        }

        public Task SaveSnapshotAsync(Guid id, long version, string aggregateJson, CancellationToken cancellationToken = default)
        {
            _logger.LogWarning("StateOnly: SaveSnapshotAsync called but snapshots are not used in StateOnly strategy");
            return Task.CompletedTask;
        }

        public Task<int> GetEventCountAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // В StateOnly хранится только текущее состояние
            return Task.FromResult(0);
        }
    }
}
