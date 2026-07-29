using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
using AwesomeAssertions.ArgumentMatchers.Moq;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class SaveProfileHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IProfileRepository> _profileRepository;
    private readonly IRequestHandler<SaveProfile, Profile> _sut;

    public SaveProfileHandlerShould()
    {
        _profileRepository = _mr.Create<IProfileRepository>();
        _sut = new SaveProfileHandler(_profileRepository.Object);
    }

    [Fact]
    public async Task SaveProfile()
    {
        var profile = Models.GetProfile("aa@aa.com");
        _profileRepository.Setup(a => a.Upsert(Its.EquivalentTo(profile), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);

        await _sut.Handle(new(profile), TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }
}
