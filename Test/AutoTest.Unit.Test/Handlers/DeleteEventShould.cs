using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class DeleteEventShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IEventsRepository> _events;
    private readonly IRequestHandler<DeleteEvent> _sut;

    public DeleteEventShould()
    {
        _events = _mr.Create<IEventsRepository>();
        _sut = new DeleteEventHandler(_events.Object);
    }

    [Fact]
    public async Task DeleteEvent()
    {
        var eventId = 1ul;
        var @event = Models.GetEvent(eventId);
        _events.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(@event).Verifiable(Times.Once);
        _events.Setup(a => a.Delete(@event, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);

        await _sut.Handle(new(eventId), TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }
}
