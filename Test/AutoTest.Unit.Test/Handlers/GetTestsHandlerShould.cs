using System.Collections.Generic;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
using AwesomeAssertions;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class GetTestsHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IEventsRepository> _eventsRepository;
    private readonly IRequestHandler<GetTests, IEnumerable<Domain.StorageModels.Course>> sut;

    public GetTestsHandlerShould()
    {
        _eventsRepository = _mr.Create<IEventsRepository>();
        sut = new GetTestsHandler(_eventsRepository.Object);
    }

    [Fact]
    public async Task GetTests()
    {
        var @event = Models.GetEvent(1);
        @event.SetCourses(new[] { new Course(0, "a") });
        _eventsRepository.Setup(a => a.GetById(1, TestContext.Current.CancellationToken)).ReturnsAsync(@event).Verifiable(Times.Once);

        var tests = await sut.Handle(new(1), TestContext.Current.CancellationToken);

        tests.Should().BeEquivalentTo(new[] { new Course(0, "a") });
        _mr.VerifyAll();
    }
}
