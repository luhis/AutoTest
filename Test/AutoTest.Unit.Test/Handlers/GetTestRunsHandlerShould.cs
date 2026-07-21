using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AwesomeAssertions;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class GetTestRunsHandlerShould
{
    private readonly MockRepository _mr;
    private readonly IRequestHandler<GetTestRuns, IEnumerable<TestRun>> _sut;
    private readonly Mock<ITestRunsRepository> _testRunsRepository;

    public GetTestRunsHandlerShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
        _testRunsRepository = _mr.Create<ITestRunsRepository>();
        _sut = new GetTestRunsHandler(_testRunsRepository.Object);
    }

    [Fact]
    public async Task ReturnTestRuns()
    {
        var eventId = 1ul;
        var ordinal = 2;
        var testRuns = new[]
        {
            new TestRun(1, eventId, ordinal, 50_000, 1, new System.DateTime(2000, 1, 1), 1),
        };
        _testRunsRepository.Setup(a => a.GetAll(eventId, ordinal, TestContext.Current.CancellationToken)).ReturnsAsync(testRuns);

        var res = (await _sut.Handle(new(eventId, ordinal), TestContext.Current.CancellationToken)).ToArray();

        res.Should().BeEquivalentTo(testRuns);
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ReturnEmptyWhenNoTestRuns()
    {
        var eventId = 1ul;
        var ordinal = 2;
        _testRunsRepository.Setup(a => a.GetAll(eventId, ordinal, TestContext.Current.CancellationToken)).ReturnsAsync(Enumerable.Empty<TestRun>());

        var res = (await _sut.Handle(new(eventId, ordinal), TestContext.Current.CancellationToken)).ToArray();

        res.Should().BeEmpty();
        _mr.VerifyAll();
    }
}
