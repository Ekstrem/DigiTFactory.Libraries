using DigiTFactory.Libraries.CommandRepository.Postgres.Configuration;
using DigiTFactory.Libraries.CommandRepository.Postgres.Entities;
using DigiTFactory.Libraries.CommandRepository.Postgres.Repositories;
using DigiTFactory.Libraries.SeedWorks.Definition;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace DigiTFactory.Libraries.CommandRepository.Postgres.Tests;

public sealed class EventStoreRebuildServiceTests
{
    [Fact]
    public async Task Rebuild_WhenEventsExist_ShouldProjectInVersionOrder()
    {
        var id = Guid.NewGuid();
        await using var db = new EventStoreDbContext(
            new DbContextOptionsBuilder<EventStoreDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options,
            new EventStoreOptions());

        db.Events.AddRange(
            NewEvent(id, 3),
            NewEvent(id, 1),
            NewEvent(id, 2));
        await db.SaveChangesAsync();

        var projector = new RecordingProjector();
        var rebuild = new EventStoreRebuildService<TestContext>(
            db, projector, NullLogger<EventStoreRebuildService<TestContext>>.Instance);

        await rebuild.RebuildAsync();

        Assert.Equal(new long[] { 1, 2, 3 }, projector.Versions);
    }

    private static DomainEventEntry NewEvent(Guid id, long version)
        => new()
        {
            Id = id,
            Version = version,
            CorrelationToken = Guid.NewGuid(),
            BoundedContext = "Test",
            CommandName = "Test",
            ChangedValueObjectsJson = "{}",
            Result = "Success"
        };

    private sealed class TestContext : IBoundedContext { }

    private sealed class RecordingProjector : IEventProjector
    {
        public List<long> Versions { get; } = new();

        public Task ProjectAsync(DomainEventEntry entry, CancellationToken ct = default)
        {
            Versions.Add(entry.Version);
            return Task.CompletedTask;
        }
    }
}
