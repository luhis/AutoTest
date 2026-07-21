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
    private readonly IRequestHandler<SaveProfile, Profile> _sut;
    private readonly MockRepository _mr;
    private readonly Mock<IProfileRepository> _profileRepository;

    public SaveProfileHandlerShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
        _profileRepository = _mr.Create<IProfileRepository>();
        _sut = new SaveProfileHandler(_profileRepository.Object);
    }

    [Fact]
    public async Task SaveProfile()
    {
        var profile = Models.GetProfile("aa@aa.com");
        _profileRepository.Setup(a => a.Upsert(Its.EquivalentTo(profile), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        await _sut.Handle(new(profile), TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }
}
