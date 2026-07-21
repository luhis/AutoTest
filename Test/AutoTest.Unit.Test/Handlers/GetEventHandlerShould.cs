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

public class GetEventHandlerShould
{
    private readonly MockRepository _mr;
    private readonly IRequestHandler<GetEvent, Event?> _sut;
    private readonly Mock<IEventsRepository> _eventsRepository;

    public GetEventHandlerShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
        _eventsRepository = _mr.Create<IEventsRepository>();
        _sut = new GetEventHandler(_eventsRepository.Object);
    }

    [Fact]
    public async Task ReturnNullWhenNotFound()
    {
        var eventId = 1ul;
        _eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync((Event?)null);

        var res = await _sut.Handle(new(eventId), TestContext.Current.CancellationToken);

        res.Should().BeNull();
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ReturnEvent()
    {
        var eventId = 1ul;
        var evnt = Models.GetEvent(eventId);
        _eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(evnt);

        var res = await _sut.Handle(new(eventId), TestContext.Current.CancellationToken);

        res.Should().BeEquivalentTo(evnt);
        _mr.VerifyAll();
    }
}
