using DigiTFactory.Libraries.EventBus.InMemory;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Events;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace DigiTFactory.Libraries.EventBus.InMemory.Tests;

public sealed class InMemoryEventBusDurableTests
{
    [Fact]
    public async Task PublishAsync_WaitsForDurableHandler_ThenDispatchesProjection()
    {
        var bus = new InMemoryEventBus(NullLogger<InMemoryEventBus>.Instance);
        var store = new TestDurableHandler();
        var projection = new TestProjection();

        bus.Subscribe<TestContext>(store);
        bus.Subscribe<TestContext>(projection);

        await bus.PublishAsync(NewEvent());

        Assert.True(store.Saved);
        Assert.True(projection.Projected);
        Assert.Equal(1, store.HandleCalls);
    }

    [Fact]
    public async Task PublishAsync_DurableFailure_PropagatesAndSkipsProjection()
    {
        var bus = new InMemoryEventBus(NullLogger<InMemoryEventBus>.Instance);
        var projection = new TestProjection();
        bus.Subscribe<TestContext>(new FailingDurableHandler());
        bus.Subscribe<TestContext>(projection);

        await Assert.ThrowsAsync<InvalidOperationException>(() => bus.PublishAsync(NewEvent()));
        Assert.False(projection.Projected);
    }

    [Fact]
    public async Task PublishAsync_DurableHandlerWithObserver_DeliversOnlyOnce()
    {
        var bus = new InMemoryEventBus(NullLogger<InMemoryEventBus>.Instance);
        var store = new TestDurableHandler();
        bus.Subscribe<TestContext>(store);

        await bus.PublishAsync(NewEvent());

        Assert.Equal(1, store.HandleCalls);
        Assert.Equal(0, store.ObserverCalls);
    }

    private static DomainEvent<TestContext> NewEvent()
        => new(
            Guid.NewGuid(), 1,
            CommandToAggregate.Commit(Guid.NewGuid(), "test", "test"),
            new Dictionary<string, IValueObject>(),
            BoundedContext<TestContext>.GetInfo(),
            DomainOperationResultEnum.Success,
            string.Empty);

    private sealed class TestContext : IBoundedContext { }

    private sealed class TestDurableHandler : IAsyncDomainEventHandler<TestContext>,
        IObserver<IDomainEvent<TestContext>>
    {
        public bool Saved { get; private set; }
        public int HandleCalls { get; private set; }
        public int ObserverCalls { get; private set; }

        public async Task HandleAsync(IDomainEvent<TestContext> domainEvent)
        {
            await Task.Delay(25);
            HandleCalls++;
            Saved = true;
        }

        public void OnNext(IDomainEvent<TestContext> value) => ObserverCalls++;
        public void OnError(Exception error) { }
        public void OnCompleted() { }
    }

    private sealed class FailingDurableHandler : IAsyncDomainEventHandler<TestContext>
    {
        public Task HandleAsync(IDomainEvent<TestContext> domainEvent)
            => throw new InvalidOperationException("event store unavailable");
    }

    private sealed class TestProjection : IDomainEventHandler<TestContext>, IObserver<IDomainEvent<TestContext>>
    {
        public bool Projected { get; private set; }
        public void OnNext(IDomainEvent<TestContext> value) => Projected = true;
        public void OnError(Exception error) { }
        public void OnCompleted() { }
    }
}
