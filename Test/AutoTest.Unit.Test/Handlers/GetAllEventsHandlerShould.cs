using System.Collections.Generic;
using System.Linq;
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

public class GetAllEventsHandlerShould
{
    private readonly MockRepository _mr;
    private readonly IRequestHandler<GetAllEvents, IEnumerable<Event>> _sut;
    private readonly Mock<IEventsRepository> _eventsRepository;

    public GetAllEventsHandlerShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
        _eventsRepository = _mr.Create<IEventsRepository>();
        _sut = new GetAllEventsHandler(_eventsRepository.Object);
    }

    [Fact]
    public async Task ReturnEvents()
    {
        var events = new[] { Models.GetEvent(1), Models.GetEvent(2) };
        _eventsRepository.Setup(a => a.GetAll(TestContext.Current.CancellationToken)).ReturnsAsync(events);

        var res = (await _sut.Handle(new(), TestContext.Current.CancellationToken)).ToArray();

        res.Should().BeEquivalentTo(events);
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ReturnEmptyWhenNoEvents()
    {
        _eventsRepository.Setup(a => a.GetAll(TestContext.Current.CancellationToken)).ReturnsAsync(Enumerable.Empty<Event>());

        var res = (await _sut.Handle(new(), TestContext.Current.CancellationToken)).ToArray();

        res.Should().BeEmpty();
        _mr.VerifyAll();
    }
}
