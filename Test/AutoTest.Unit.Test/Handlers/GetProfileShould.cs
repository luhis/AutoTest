using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
using AwesomeAssertions;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

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
        profileRepository.Setup(a => a.Get(email, TestContext.Current.CancellationToken)).ReturnsAsync((Profile?)null);

        var res = await sut.Handle(new(email), TestContext.Current.CancellationToken);

        res.EmailAddress.Should().Be(email);
        mr.VerifyAll();
    }

    [Fact]
    public async Task ReturnExistingProfileIfSome()
    {
        var email = "a@a.com";
        var profile = Models.GetProfile(email);
        profileRepository.Setup(a => a.Get(email, TestContext.Current.CancellationToken)).ReturnsAsync(profile);

        var res = await sut.Handle(new(email), TestContext.Current.CancellationToken);

        res.Should().BeEquivalentTo(profile);
        mr.VerifyAll();
    }
}
