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
    public class DeleteClubShould
    {
        private readonly IRequestHandler<DeleteClub> sut;
        private readonly MockRepository mr;
        private readonly Mock<IClubsRepository> clubs;

        public DeleteClubShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            clubs = mr.Create<IClubsRepository>();
            sut = new DeleteClubHandler(clubs.Object);
        }

        [Fact]
        public async Task ReturnBlankProfileIfNone()
        {
            var clubId = 1ul;
            clubs.Setup(a => a.Delete(clubId, CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new(clubId), CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
