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
    private readonly MockRepository _mr;
    private readonly Mock<IEventsRepository> _eventsRepository;
    private readonly Mock<IEntrantsRepository> _entrantsRepository;
    private readonly Mock<ITestRunsRepository> _testRunsRepository;
    private readonly Mock<ITotalTimeCalculator> _totalTimeCalculator;

    public CompetitionDataShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
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
            new Event(eventId, 1, "Farm", DateTime.MinValue, 1, 1, "", new[] { EventType.AutoTest }, "", TimingSystem.StopWatch, DateTime.MinValue, DateTime.MaxValue, 50, DateTime.MinValue));
        _entrantsRepository.Setup(r => r.GetByEventId(eventId, cancellationToken)).ReturnsAsync(new[] { entrant });
        _testRunsRepository.Setup(r => r.GetAll(eventId, cancellationToken)).ReturnsAsync(Array.Empty<TestRun>());
        _totalTimeCalculator.Setup(r => r.GetTotalTime(It.IsAny<TimeCalculatorConfig>(), It.IsAny<IEnumerable<TestRun>>(), It.IsAny<IEnumerable<TestRun>>())).Returns(0);

        var result = await CompetitionData.GetEntrantsAndRuns(eventId, _eventsRepository.Object, _entrantsRepository.Object, _testRunsRepository.Object, _totalTimeCalculator.Object, cancellationToken);

        result.Should().HaveCount(1);
        result[0].Entrant.Should().BeSameAs(entrant);
        result[0].Runs.Should().BeEmpty();
        result[0].TotalTime.Should().Be(0);
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
            new Event(eventId, 1, "Farm", DateTime.MinValue, 1, 1, "", new[] { EventType.AutoTest }, "", TimingSystem.StopWatch, DateTime.MinValue, DateTime.MaxValue, 50, DateTime.MinValue));
        _entrantsRepository.Setup(r => r.GetByEventId(eventId, cancellationToken)).ReturnsAsync(new[] { entrant1, entrant2 });
        var run1 = new TestRun(1, eventId, 1, 5000, 1, DateTime.MinValue, 99);
        var run2 = new TestRun(2, eventId, 1, 10000, 2, DateTime.MinValue, 99);
        _testRunsRepository.Setup(r => r.GetAll(eventId, cancellationToken)).ReturnsAsync(new[] { run1, run2 });
        _totalTimeCalculator.Setup(r => r.GetTotalTime(It.IsAny<TimeCalculatorConfig>(), It.IsAny<IEnumerable<TestRun>>(), It.IsAny<IEnumerable<TestRun>>()))
            .Returns((TimeCalculatorConfig c, IEnumerable<TestRun> runs, IEnumerable<TestRun> all) =>
            {
                var first = runs.FirstOrDefault();
                if (first == null) return 0;
                return first.EntrantId == 1 ? 5000 : 10000;
            });

        var result = await CompetitionData.GetEntrantsAndRuns(eventId, _eventsRepository.Object, _entrantsRepository.Object, _testRunsRepository.Object, _totalTimeCalculator.Object, cancellationToken);

        result.Should().HaveCount(2);
        result[0].Entrant.EntrantId.Should().Be(1);
        result[0].TotalTime.Should().Be(5000);
        result[1].Entrant.EntrantId.Should().Be(2);
        result[1].TotalTime.Should().Be(10000);
        _mr.VerifyAll();
    }

    [Fact]
    public async Task TakeOnlyTwoRunsPerOrdinal()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var eventId = 10ul;
        var entrant = new Entrant(1, 1, "Joe", "Bloggs", "j@test.com", "A", eventId, Age.Senior, false, null);

        _eventsRepository.Setup(r => r.GetById(eventId, cancellationToken)).ReturnsAsync(
            new Event(eventId, 1, "Farm", DateTime.MinValue, 1, 1, "", new[] { EventType.AutoTest }, "", TimingSystem.StopWatch, DateTime.MinValue, DateTime.MaxValue, 50, DateTime.MinValue));
        _entrantsRepository.Setup(r => r.GetByEventId(eventId, cancellationToken)).ReturnsAsync(new[] { entrant });

        var runs = new[]
        {
            new TestRun(1, eventId, 1, 1000, 1, new DateTime(2024, 1, 1, 10, 0, 0), 99),
            new TestRun(2, eventId, 1, 2000, 1, new DateTime(2024, 1, 1, 10, 1, 0), 99),
            new TestRun(3, eventId, 1, 3000, 1, new DateTime(2024, 1, 1, 10, 2, 0), 99),
        };
        _testRunsRepository.Setup(r => r.GetAll(eventId, cancellationToken)).ReturnsAsync(runs);
        _totalTimeCalculator.Setup(r => r.GetTotalTime(It.IsAny<TimeCalculatorConfig>(), It.IsAny<IEnumerable<TestRun>>(), It.IsAny<IEnumerable<TestRun>>())).Returns(1000);

        var result = await CompetitionData.GetEntrantsAndRuns(eventId, _eventsRepository.Object, _entrantsRepository.Object, _testRunsRepository.Object, _totalTimeCalculator.Object, cancellationToken);

        result.Should().HaveCount(1);
        result[0].Runs.Should().HaveCount(2);
        result[0].Runs[0].Created.Should().Be(new DateTime(2024, 1, 1, 10, 0, 0));
        result[0].Runs[1].Created.Should().Be(new DateTime(2024, 1, 1, 10, 1, 0));
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ThrowWhenEventNotFound()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var eventId = 10ul;
        _eventsRepository.Setup(r => r.GetById(eventId, cancellationToken)).ReturnsAsync((Event?)null);

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

        result.Should().HaveCount(2);
        result[0].Entrant.Should().BeSameAs(entrant1);
        result[0].TotalTime.Should().Be(5000);
        result[0].Position.Should().Be(0);
        result[0].Times.Should().HaveCount(1);
        result[0].Times.First().Ordinal.Should().Be(1);

        result[1].Entrant.Should().BeSameAs(entrant2);
        result[1].TotalTime.Should().Be(13000);
        result[1].Position.Should().Be(1);
        result[1].Times.Should().HaveCount(2);
    }
}
