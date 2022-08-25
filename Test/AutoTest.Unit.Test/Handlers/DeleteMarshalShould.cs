using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using FluentAssertions.ArgumentMatchers.Moq;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class DeleteMarshalShould
    {
        private readonly IRequestHandler<DeleteMarshal> sut;
        private readonly MockRepository mr;
        private readonly Mock<IMarshalsRepository> entrants;
        private readonly Mock<ISignalRNotifier> signalRNotifier;

        public DeleteMarshalShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            entrants = mr.Create<IMarshalsRepository>();
            signalRNotifier = mr.Create<ISignalRNotifier>();
            sut = new DeleteMarshalHandler(entrants.Object, signalRNotifier.Object);
        }

        [Fact]
        public async Task ReturnBlankProfileIfNone()
        {
            var eventId = 1ul;
            var marshalId = 2ul;
            var entrant = new Domain.StorageModels.Marshal(marshalId, "joe", "bloggs", "joe@bloggs.com", eventId, 1234, "");
            entrants.Setup(a => a.GetById(eventId, marshalId, CancellationToken.None)).ReturnsAsync(entrant);
            entrants.Setup(a => a.Remove(entrant, CancellationToken.None)).Returns(Task.CompletedTask);
            signalRNotifier.Setup(a => a.RemoveEventMarshal(marshalId, Its.EquivalentTo(new[] { "joe@bloggs.com" }), CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new(eventId, marshalId), CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
