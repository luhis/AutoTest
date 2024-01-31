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
    public class GetProfileShould
    {
        private readonly MockRepository mr;
        private readonly IRequestHandler<GetProfile, Profile> sut;
        private readonly Mock<IProfileRepository> profileRepository;

        public GetProfileShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            profileRepository = mr.Create<IProfileRepository>();
            sut = new GetProfileHandler(profileRepository.Object);
        }

        [Fact]
        public async Task ReturnBlankProfileIfNone()
        {
            var email = "a@a.com";
            profileRepository.Setup(a => a.Get(email, CancellationToken.None)).ReturnsAsync((Profile?)null);

            var res = await sut.Handle(new(email), CancellationToken.None);

            res.EmailAddress.Should().BeEquivalentTo(email);
            mr.VerifyAll();
        }

        [Fact]
        public async Task ReturnExistingProfileIfSome()
        {
            var email = "a@a.com";
            var profile = new Profile(email, "First", "Last", Domain.Enums.Age.Junior, false);
            profileRepository.Setup(a => a.Get(email, CancellationToken.None)).ReturnsAsync(profile);

            var res = await sut.Handle(new(email), CancellationToken.None);

            res.Should().BeEquivalentTo(profile);
            mr.VerifyAll();
        }
    }
}
