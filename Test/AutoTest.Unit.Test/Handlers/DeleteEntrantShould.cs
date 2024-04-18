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
        public async Task DeleteEntrant()
        {
            var eventId = 1ul;
            var entrantId = 2ul;
            var entrant = Models.GetEntrant(entrantId, eventId);
            entrants.Setup(a => a.GetById(eventId, entrantId, CancellationToken.None)).ReturnsAsync(entrant);
            entrants.Setup(a => a.Delete(entrant, CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new(eventId, entrantId), CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
