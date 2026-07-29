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
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IProfileRepository> _profileRepository;
    private readonly IRequestHandler<GetProfile, Profile> _sut;

    public GetProfileShould()
    {
        _profileRepository = _mr.Create<IProfileRepository>();
        _sut = new GetProfileHandler(_profileRepository.Object);
    }

    [Fact]
    public async Task ReturnBlankProfileIfNone()
    {
        var email = "a@a.com";
        _profileRepository.Setup(a => a.Get(email, TestContext.Current.CancellationToken)).ReturnsAsync((Profile?)null).Verifiable(Times.Once);

        var res = await _sut.Handle(new(email), TestContext.Current.CancellationToken);

        res.EmailAddress.Should().Be(email);
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ReturnExistingProfileIfSome()
    {
        var email = "a@a.com";
        var profile = Models.GetProfile(email);
        _profileRepository.Setup(a => a.Get(email, TestContext.Current.CancellationToken)).ReturnsAsync(profile).Verifiable(Times.Once);

        var res = await _sut.Handle(new(email), TestContext.Current.CancellationToken);

        res.Should().BeEquivalentTo(profile);
        _mr.VerifyAll();
    }
}
