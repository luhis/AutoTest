using System.Threading;
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
    private readonly IRequestHandler<SaveProfile, Profile> sut;
    private readonly MockRepository mr;
    private readonly Mock<IProfileRepository> profileRepository;

    public SaveProfileHandlerShould()
    {
        mr = new MockRepository(MockBehavior.Strict);
        profileRepository = mr.Create<IProfileRepository>();
        sut = new SaveProfileHandler(profileRepository.Object);
    }

    [Fact]
    public async Task SaveProfile()
    {
        var profile = Models.GetProfile("aa@aa.com");
        profileRepository.Setup(a => a.Upsert(Its.EquivalentTo(profile), CancellationToken.None)).Returns(Task.CompletedTask);

        await sut.Handle(new(profile), CancellationToken.None);

        mr.VerifyAll();
    }
}
