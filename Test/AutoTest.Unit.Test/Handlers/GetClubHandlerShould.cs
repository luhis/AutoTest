using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class GetClubHandlerShould
    {
        private readonly MockRepository mr;
        private readonly IRequestHandler<GetClub, Club?> sut;
        private readonly Mock<IClubsRepository> profileRepository;

        public GetClubHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            profileRepository = mr.Create<IClubsRepository>();
            sut = new GetClubHandler(profileRepository.Object);
        }

        [Fact]
        public async Task ReturnNullIfNotClub()
        {
            var clubId = 1ul;
            profileRepository.Setup(a => a.GetById(clubId, CancellationToken.None)).ReturnsAsync((Club?)null);

            var res = await sut.Handle(new(clubId), CancellationToken.None);

            res.Should().BeNull();
            mr.VerifyAll();
        }

        [Fact]
        public async Task ReturnClub()
        {
            var clubId = 1ul;
            var club = new Club(clubId, "First", "Last", "");
            profileRepository.Setup(a => a.GetById(clubId, CancellationToken.None)).ReturnsAsync(club);

            var res = await sut.Handle(new(clubId), CancellationToken.None);

            res.Should().BeEquivalentTo(club);
            mr.VerifyAll();
        }
    }
}
