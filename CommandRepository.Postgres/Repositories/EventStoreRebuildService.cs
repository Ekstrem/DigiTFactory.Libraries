using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DigiTFactory.Libraries.CommandRepository.Postgres.Repositories
{
    /// <summary>
    /// Библиотечная реализация IRebuildService: читает все события в порядке Version
    /// и прогоняет их через IEventProjector. Идемпотентность обеспечивает проектор.
    /// </summary>
    public sealed class EventStoreRebuildService<TBoundedContext> : IRebuildService<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly EventStoreDbContext _dbContext;
        private readonly IEventProjector _projector;
        private readonly ILogger<EventStoreRebuildService<TBoundedContext>> _logger;

        public EventStoreRebuildService(
            EventStoreDbContext dbContext,
            IEventProjector projector,
            ILogger<EventStoreRebuildService<TBoundedContext>> logger)
        {
            _dbContext = dbContext;
            _projector = projector;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task RebuildAsync(CancellationToken ct = default)
        {
            var events = await _dbContext.Events
                .AsNoTracking()
                .OrderBy(e => e.Version)
                .ToListAsync(ct);

            _logger.LogInformation("Rebuild {BoundedContext}: {EventCount} events", typeof(TBoundedContext).Name, events.Count);

            foreach (var entry in events)
            {
                ct.ThrowIfCancellationRequested();
                await _projector.ProjectAsync(entry, ct);
            }
        }
    }
}
