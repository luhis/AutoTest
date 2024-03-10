using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
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
        private readonly Mock<IFileRepository> files;

        public DeleteEventShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            events = mr.Create<IEventsRepository>();
            files = mr.Create<IFileRepository>();
            sut = new DeleteEventHandler(events.Object, files.Object);
        }

        [Fact]
        public async Task ReturnBlankProfileIfNone()
        {
            var eventId = 1ul;
            var @event = Models.GetEvent(eventId);
            events.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(@event);
            events.Setup(a => a.Delete(@event, CancellationToken.None)).Returns(Task.CompletedTask);
            files.Setup(a => a.DeleteMaps(eventId, CancellationToken.None)).Returns(Task.CompletedTask);
            files.Setup(a => a.DeleteRegs(eventId, CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new(eventId), CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
