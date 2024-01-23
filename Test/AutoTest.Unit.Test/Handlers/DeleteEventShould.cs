using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
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
        public async Task ReturnBlankProfileIfNone()
        {
            var eventId = 1ul;
            var @event = new Event(eventId, 1, "", new System.DateTime(), 1, 1, "", new EventType[] { }, "", TimingSystem.StopWatch, new System.DateTime(), new System.DateTime(), 2);
            events.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(@event);
            events.Setup(a => a.Delete(@event, CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new(eventId), CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
