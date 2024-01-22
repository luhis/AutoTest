using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class DeleteEntrantShould
    {
        private readonly IRequestHandler<DeleteEntrant> sut;
        private readonly MockRepository mr;
        private readonly Mock<IEntrantsRepository> entrants;

        public DeleteEntrantShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            entrants = mr.Create<IEntrantsRepository>();
            sut = new DeleteEntrantHandler(entrants.Object);
        }

        [Fact]
        public async Task ReturnBlankProfileIfNone()
        {
            var eventId = 1ul;
            var entrantId = 2ul;
            var entrant = new Domain.StorageModels.Entrant(entrantId, 22, "joe", "bloggs", "joe@bloggs.com", Domain.Enums.EventType.AutoTest, "A", eventId, "BRMC", 1234, Domain.Enums.Age.Senior);
            entrants.Setup(a => a.GetById(eventId, entrantId, CancellationToken.None)).ReturnsAsync(entrant);
            entrants.Setup(a => a.Delete(entrant, CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new(eventId, entrantId), CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
