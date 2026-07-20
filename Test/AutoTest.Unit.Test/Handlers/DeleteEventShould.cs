using System.Threading;
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
    private readonly IRequestHandler<DeleteEvent> sut;
    private readonly MockRepository mr;
    private readonly Mock<IEventsRepository> events;

    public DeleteEventShould()
    {
        mr = new MockRepository(MockBehavior.Strict);
        events = mr.Create<IEventsRepository>();
        sut = new DeleteEventHandler(events.Object);
    }

    [Fact]
    public async Task DeleteEvent()
    {
        var eventId = 1ul;
        var @event = Models.GetEvent(eventId);
        events.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(@event);
        events.Setup(a => a.Delete(@event, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        await sut.Handle(new(eventId), TestContext.Current.CancellationToken);

        mr.VerifyAll();
    }
}
