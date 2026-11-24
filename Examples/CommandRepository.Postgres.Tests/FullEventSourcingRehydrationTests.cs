using DigiTFactory.Libraries.CommandRepository.Postgres.Entities;
using DigiTFactory.Libraries.CommandRepository.Postgres.Configuration;
using DigiTFactory.Libraries.CommandRepository.Postgres.Repositories;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace DigiTFactory.Libraries.CommandRepository.Postgres.Tests;

public sealed class FullEventSourcingRehydrationTests
{
    [Fact]
    public async Task FullEs_WhenEventsExist_RehydratesLatestAndHistoricalStates()
    {
        var id = Guid.NewGuid();
        var secondToken = Guid.NewGuid();
        await using var db = NewContext();
        db.Events.AddRange(
            NewEvent(id, 1, Guid.NewGuid()),
            NewEvent(id, 2, secondToken),
            NewEvent(id, 3, Guid.NewGuid()));
        await db.SaveChangesAsync();

        var repository = new FullEventSourcingRepository<TestContext, TestModel>(
            db,
            NullLogger<FullEventSourcingRepository<TestContext, TestModel>>.Instance,
            new TestRehydrator());

        var stream = await repository.GetById(id, default);
        var historical = await repository.GetByIdAndVersion(id, 2, default);
        var byCorrelation = await repository.GetByCorrelationToken(secondToken, default);

        Assert.Equal(new long[] { 1, 2, 3 }, stream.Select(m => m.Version));
        Assert.Equal(2, historical.Version);
        Assert.Equal(2, byCorrelation.Version);
    }

    private static EventStoreDbContext NewContext()
        => new(new DbContextOptionsBuilder<EventStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options, new EventStoreOptions());

    private static DomainEventEntry NewEvent(Guid id, long version, Guid correlationToken)
        => new()
        {
            Id = id,
            Version = version,
            CorrelationToken = correlationToken,
            BoundedContext = "Test",
            CommandName = "Test",
            ChangedValueObjectsJson = "{}",
            Result = "Success"
        };

    private sealed class TestContext : IBoundedContext { }

    private sealed class TestRehydrator : IEventStreamRehydrator<TestContext, TestModel>
    {
        public IReadOnlyList<TestModel> Rehydrate(IReadOnlyList<DomainEventEntry> events)
            => events.Select(e => new TestModel
            {
                Id = e.Id,
                Version = e.Version,
                CorrelationToken = e.CorrelationToken,
                CommandName = e.CommandName
            }).ToList();
    }

    private sealed class TestModel : IAnemicModel<TestContext>
    {
        public Guid Id { get; init; }
        public long Version { get; init; }
        public Guid CorrelationToken { get; init; }
        public string CommandName { get; init; } = string.Empty;
        public string SubjectName { get; init; } = string.Empty;
        public IDictionary<string, IValueObject> Invariants { get; init; } =
            new Dictionary<string, IValueObject>();
        public IDictionary<string, IValueObject> GetValueObjects() => Invariants;
    }
}
