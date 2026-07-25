using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Service.Models;
using AutoTest.Service.ResultCalculation;
using AutoTest.Unit.Test.MockData;
using AwesomeAssertions;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class GetResultsHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<ITestRunsRepository> _testRunsRepository;
    private readonly Mock<IEventsRepository> _eventsRepository;
    private readonly Mock<IEntrantsRepository> _entrantsRepository;
    private readonly Mock<ITotalTimeCalculator> _totalTimeCalculator;
    private readonly IRequestHandler<GetResults, IEnumerable<Result>> _sut;

    public GetResultsHandlerShould()
    {
        _testRunsRepository = _mr.Create<ITestRunsRepository>();
        _eventsRepository = _mr.Create<IEventsRepository>();
        _entrantsRepository = _mr.Create<IEntrantsRepository>();
        _totalTimeCalculator = _mr.Create<ITotalTimeCalculator>();
        _sut = new GetResultsHandler(_testRunsRepository.Object, _eventsRepository.Object, _entrantsRepository.Object, _totalTimeCalculator.Object);
    }

    [Fact]
    public async Task GetResults()
    {
        var entrantId = 1ul;
        var eventId = 22ul;

        _eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(
            Models.GetEvent(eventId)
            );
        var entrant = Models.GetEntrant(entrantId, eventId);
        var entrant2 = Models.GetEntrant(entrantId + 1, eventId);
        _entrantsRepository.Setup(a => a.GetByEventId(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(new[] { entrant, entrant2 });
        _testRunsRepository.Setup(a => a.GetAll(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(Enumerable.Empty<TestRun>());
        _totalTimeCalculator.Setup(a => a.GetTotalTime(It.IsAny<AutoTest.Service.ResultCalculation.TimeCalculatorConfig>(), It.IsAny<IEnumerable<TestRun>>(), It.IsAny<IEnumerable<TestRun>>())).Returns(0);

        var results = await _sut.Handle(new(eventId), TestContext.Current.CancellationToken);

        results.Should().HaveCount(1);
        results.Should().BeEquivalentTo(new[] { new Result("A", new[] {
            new EntrantTimes(entrant, 0, Enumerable.Empty<TestTime>(), 0, 0),
            new EntrantTimes(entrant2, 0, Enumerable.Empty<TestTime>(), 1, 1)
        }) });

        _mr.VerifyAll();
    }
}
