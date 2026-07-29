using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Models;
using AutoTest.Service.ResultCalculation;
using AwesomeAssertions;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class CompetitionDataShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IEventsRepository> _eventsRepository;
    private readonly Mock<IEntrantsRepository> _entrantsRepository;
    private readonly Mock<ITestRunsRepository> _testRunsRepository;
    private readonly Mock<ITotalTimeCalculator> _totalTimeCalculator;

    public CompetitionDataShould()
    {
        _eventsRepository = _mr.Create<IEventsRepository>();
        _entrantsRepository = _mr.Create<IEntrantsRepository>();
        _testRunsRepository = _mr.Create<ITestRunsRepository>();
        _totalTimeCalculator = _mr.Create<ITotalTimeCalculator>();
    }

    [Fact]
    public async Task ReturnEntrantWithNoRuns()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var eventId = 10ul;
        var entrant = new Entrant(1, 1, "Joe", "Bloggs", "j@test.com", "A", eventId, Age.Senior, false, null);

        _eventsRepository.Setup(r => r.GetById(eventId, cancellationToken)).ReturnsAsync(
            new Event(eventId, 1, "Farm", DateTime.MinValue, 1, 1, new[] { EventType.AutoTest }, TimingSystem.StopWatch, DateTime.MinValue, DateTime.MaxValue, 50, DateTime.MinValue)).Verifiable(Times.Once);
        _entrantsRepository.Setup(r => r.GetAll(eventId, cancellationToken)).ReturnsAsync(new[] { entrant }).Verifiable(Times.Once);
        _testRunsRepository.Setup(r => r.GetAll(eventId, cancellationToken)).ReturnsAsync(Array.Empty<TestRun>()).Verifiable(Times.Once);
        _totalTimeCalculator.Setup(r => r.GetTotalTime(It.IsAny<TimeCalculatorConfig>(), It.IsAny<IEnumerable<TestRun>>(), It.IsAny<IEnumerable<TestRun>>())).Returns(0).Verifiable(Times.Once);

        var result = await CompetitionData.GetEntrantsAndRuns(eventId, _eventsRepository.Object, _entrantsRepository.Object, _testRunsRepository.Object, _totalTimeCalculator.Object, cancellationToken);

        result.Should().BeEquivalentTo(new[] { new { Entrant = entrant, Runs = Array.Empty<TestRun>(), TotalTime = 0 } },
            o => o.WithStrictOrdering());
        _mr.VerifyAll();
    }

    [Fact]
    public async Task OrderByTotalTime()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var eventId = 10ul;
        var entrant1 = new Entrant(1, 1, "Fast", "Driver", "f@test.com", "A", eventId, Age.Senior, false, null);
        var entrant2 = new Entrant(2, 2, "Slow", "Driver", "s@test.com", "A", eventId, Age.Senior, false, null);

        _eventsRepository.Setup(r => r.GetById(eventId, cancellationToken)).ReturnsAsync(
            new Event(eventId, 1, "Farm", DateTime.MinValue, 1, 1, new[] { EventType.AutoTest }, TimingSystem.StopWatch, DateTime.MinValue, DateTime.MaxValue, 50, DateTime.MinValue)).Verifiable(Times.Once);
        _entrantsRepository.Setup(r => r.GetAll(eventId, cancellationToken)).ReturnsAsync(new[] { entrant1, entrant2 }).Verifiable(Times.Once);
        var run1 = new TestRun(1, eventId, 1, 5000, 1, DateTime.MinValue, 99);
        var run2 = new TestRun(2, eventId, 1, 10000, 2, DateTime.MinValue, 99);
        _testRunsRepository.Setup(r => r.GetAll(eventId, cancellationToken)).ReturnsAsync(new[] { run1, run2 }).Verifiable(Times.Once);
        _totalTimeCalculator.Setup(r => r.GetTotalTime(It.IsAny<TimeCalculatorConfig>(), It.IsAny<IEnumerable<TestRun>>(), It.IsAny<IEnumerable<TestRun>>()))
            .Returns((TimeCalculatorConfig c, IEnumerable<TestRun> runs, IEnumerable<TestRun> all) =>
            {
                var first = runs.FirstOrDefault();
                if (first == null) return 0;
                return first.EntrantId == 1 ? 5000 : 10000;
            }).Verifiable(Times.AtLeastOnce);

        var result = await CompetitionData.GetEntrantsAndRuns(eventId, _eventsRepository.Object, _entrantsRepository.Object, _testRunsRepository.Object, _totalTimeCalculator.Object, cancellationToken);

        result.Should().BeEquivalentTo(new[]
        {
            new { Entrant = new { EntrantId = 1UL }, TotalTime = 5000 },
            new { Entrant = new { EntrantId = 2UL }, TotalTime = 10000 },
        }, o => o.WithStrictOrdering());
        _mr.VerifyAll();
    }

    [Fact]
    public async Task TakeOnlyTwoRunsPerOrdinal()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var eventId = 10ul;
        var entrant = new Entrant(1, 1, "Joe", "Bloggs", "j@test.com", "A", eventId, Age.Senior, false, null);

        _eventsRepository.Setup(r => r.GetById(eventId, cancellationToken)).ReturnsAsync(
            new Event(eventId, 1, "Farm", DateTime.MinValue, 1, 1, new[] { EventType.AutoTest }, TimingSystem.StopWatch, DateTime.MinValue, DateTime.MaxValue, 50, DateTime.MinValue)).Verifiable(Times.Once);
        _entrantsRepository.Setup(r => r.GetAll(eventId, cancellationToken)).ReturnsAsync(new[] { entrant }).Verifiable(Times.Once);

        var runs = new[]
        {
            new TestRun(1, eventId, 1, 1000, 1, new DateTime(2024, 1, 1, 10, 0, 0), 99),
            new TestRun(2, eventId, 1, 2000, 1, new DateTime(2024, 1, 1, 10, 1, 0), 99),
            new TestRun(3, eventId, 1, 3000, 1, new DateTime(2024, 1, 1, 10, 2, 0), 99),
        };
        _testRunsRepository.Setup(r => r.GetAll(eventId, cancellationToken)).ReturnsAsync(runs).Verifiable(Times.Once);
        _totalTimeCalculator.Setup(r => r.GetTotalTime(It.IsAny<TimeCalculatorConfig>(), It.IsAny<IEnumerable<TestRun>>(), It.IsAny<IEnumerable<TestRun>>())).Returns(1000).Verifiable(Times.Once);

        var result = await CompetitionData.GetEntrantsAndRuns(eventId, _eventsRepository.Object, _entrantsRepository.Object, _testRunsRepository.Object, _totalTimeCalculator.Object, cancellationToken);

        result.Should().BeEquivalentTo(new[]
        {
            new { Runs = new[] { new { Created = new DateTime(2024, 1, 1, 10, 0, 0) }, new { Created = new DateTime(2024, 1, 1, 10, 1, 0) } } },
        }, o => o.WithStrictOrdering());
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ThrowWhenEventNotFound()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var eventId = 10ul;
        _eventsRepository.Setup(r => r.GetById(eventId, cancellationToken)).ReturnsAsync((Event?)null).Verifiable(Times.Once);

        var act = () => CompetitionData.GetEntrantsAndRuns(eventId, _eventsRepository.Object, _entrantsRepository.Object, _testRunsRepository.Object, _totalTimeCalculator.Object, cancellationToken);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Event not found");
        _mr.VerifyAll();
    }

    [Fact]
    public void MapToEntrantTimesCorrectly()
    {
        var eventId = 10ul;
        var entrant1 = new Entrant(1, 1, "Joe", "Bloggs", "j@test.com", "A", eventId, Age.Senior, false, null);
        var entrant2 = new Entrant(2, 2, "Jane", "Doe", "j2@test.com", "B", eventId, Age.Senior, false, null);

        var courses = new[] { new Course(1, "map1.png"), new Course(2, "map2.png") };

        var runs1 = new[]
        {
            new TestRun(1, eventId, 1, 5000, 1, DateTime.MinValue, 99),
        };
        var runs2 = new[]
        {
            new TestRun(2, eventId, 1, 7000, 2, DateTime.MinValue, 99),
            new TestRun(3, eventId, 2, 6000, 2, DateTime.MinValue, 99),
        };

        var entrantsAndRuns = new[]
        {
            new CompetitionData.EntrantRuns(entrant1, runs1, 5000),
            new CompetitionData.EntrantRuns(entrant2, runs2, 13000),
        };

        var result = CompetitionData.ToEntrantTimes(entrantsAndRuns, courses);

        result.Should().BeEquivalentTo(new[]
        {
            new { Entrant = entrant1, TotalTime = 5000, Position = 0, Times = new[] { new { Ordinal = 1 } } },
            new { Entrant = entrant2, TotalTime = 13000, Position = 1, Times = new[] { new { Ordinal = 1 }, new { Ordinal = 2 } } },
        }, o => o.WithStrictOrdering());
    }
}
