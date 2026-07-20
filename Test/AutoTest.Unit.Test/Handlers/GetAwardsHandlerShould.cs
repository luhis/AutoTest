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

public class GetAwardsHandlerShould
{
    private readonly IRequestHandler<GetAwards, Awards> sut;
    private readonly MockRepository mr;
    private readonly Mock<ITestRunsRepository> testRunsRepository;
    private readonly Mock<IEventsRepository> eventsRepository;
    private readonly Mock<IEntrantsRepository> entrantsRepository;
    private readonly Mock<ITotalTimeCalculator> totalTimeCalculator;

    public GetAwardsHandlerShould()
    {
        mr = new MockRepository(MockBehavior.Strict);
        testRunsRepository = mr.Create<ITestRunsRepository>();
        eventsRepository = mr.Create<IEventsRepository>();
        entrantsRepository = mr.Create<IEntrantsRepository>();
        totalTimeCalculator = mr.Create<ITotalTimeCalculator>();
        sut = new GetAwardsHandler(testRunsRepository.Object, eventsRepository.Object, entrantsRepository.Object, totalTimeCalculator.Object);
    }

    [Fact]
    public async Task GetAwards()
    {
        var entrantId = 1ul;
        var eventId = 22ul;

        eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(
            Models.GetEvent(eventId)
            );
        var entrant = Models.GetEntrant(entrantId, eventId);
        var entrant2 = Models.GetEntrant(entrantId + 1, eventId);
        entrantsRepository.Setup(a => a.GetByEventId(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(new[] { entrant, entrant2 });
        testRunsRepository.Setup(a => a.GetAll(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(Enumerable.Empty<TestRun>());
        totalTimeCalculator.Setup(a => a.GetTotalTime(It.IsAny<AutoTest.Service.ResultCalculation.TimeCalculatorConfig>(), It.IsAny<IEnumerable<TestRun>>(), It.IsAny<IEnumerable<TestRun>>())).Returns(0);

        var res = await sut.Handle(new(eventId), TestContext.Current.CancellationToken);

        res.Should().BeEquivalentTo(new Awards(new EntrantTimes(entrant, 0, Enumerable.Empty<TestTime>(), 0, 0), new[] {
            new Result("A", new[] { new EntrantTimes(entrant2, 0, Enumerable.Empty<TestTime>(), 1, 1) })
        }));

        mr.VerifyAll();
    }
}
