using System.Linq;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence.Repositories;
using AutoTest.Unit.Test.Fixtures;
using AutoTest.Unit.Test.MockData;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class TestRunsRepositoryShould
{
    [Fact]
    public async Task GetAllReturnsAllRunsForEvent()
    {
        using var db = InMemDbFixture.GetDbContext();
        ITestRunsRepository sut = new TestRunsRepository(db);

        db.TestRuns.AddRange(
            Models.GetTestRun(1, 10),
            Models.GetTestRun(2, 10),
            Models.GetTestRun(3, 20));
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = (await sut.GetAll(10, TestContext.Current.CancellationToken)).ToArray();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllByOrdinalReturnsRunsForOrdinal()
    {
        using var db = InMemDbFixture.GetDbContext();
        ITestRunsRepository sut = new TestRunsRepository(db);

        db.TestRuns.AddRange(
            Models.GetTestRun(1, 10, testNumber: 1),
            Models.GetTestRun(2, 10, testNumber: 1),
            Models.GetTestRun(3, 10, testNumber: 2));
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = (await sut.GetAll(10, 1, TestContext.Current.CancellationToken)).ToArray();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllReturnsEmptyWhenNoRuns()
    {
        using var db = InMemDbFixture.GetDbContext();
        ITestRunsRepository sut = new TestRunsRepository(db);

        var result = (await sut.GetAll(999, TestContext.Current.CancellationToken)).ToArray();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AddTestRunAddsToDatabase()
    {
        using var db = InMemDbFixture.GetDbContext();
        ITestRunsRepository sut = new TestRunsRepository(db);

        var testRun = Models.GetTestRun(1, 10);
        await sut.AddTestRun(testRun, TestContext.Current.CancellationToken);

        var result = await db.TestRuns.FindAsync(new object[] { 1UL }, TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateTestRunUpdatesDatabase()
    {
        using var db = InMemDbFixture.GetDbContext();
        ITestRunsRepository sut = new TestRunsRepository(db);

        var testRun = Models.GetTestRun(1, 10);
        db.TestRuns.Add(testRun);
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        testRun.SetPenalties(new[] { new Penalty(Domain.Enums.PenaltyEnum.Late, 1) });
        await sut.UpdateTestRun(testRun, TestContext.Current.CancellationToken);

        var result = await db.TestRuns.FindAsync(new object[] { 1UL }, TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        result!.Penalties.Should().HaveCount(1);
    }
}
