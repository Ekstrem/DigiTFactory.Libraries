using DigiTFactory.Libraries.Testing;
using Xunit;

namespace DigiTFactory.Libraries.Testing.Tests;

public sealed class ProjectionCoverageTests
{
    [Fact]
    public async Task VerifyRebuildEquivalence_WhenEqual_DoesNotThrow()
    {
        await ProjectionCoverage.VerifyRebuildEquivalence(
            () => Task.FromResult(1),
            () => Task.FromResult(1));
    }

    [Fact]
    public async Task VerifyRebuildEquivalence_WhenDiffer_Throws()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            ProjectionCoverage.VerifyRebuildEquivalence(
                () => Task.FromResult(1),
                () => Task.FromResult(2)));
    }

    [Fact]
    public async Task VerifyIdempotency_WhenStable_DoesNotThrow()
    {
        await ProjectionCoverage.VerifyIdempotency(
            () => Task.FromResult(5),
            () => Task.FromResult(5));
    }

    [Fact]
    public async Task VerifyIdempotency_WhenChanged_Throws()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            ProjectionCoverage.VerifyIdempotency(
                () => Task.FromResult(5),
                () => Task.FromResult(6)));
    }
}
