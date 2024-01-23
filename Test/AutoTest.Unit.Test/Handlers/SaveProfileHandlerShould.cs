using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using FluentAssertions.ArgumentMatchers.Moq;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class SaveProfileHandlerShould
    {
        private readonly IRequestHandler<SaveProfile, string> sut;
        private readonly MockRepository mr;
        private readonly Mock<IProfileRepository> testRuns;

        public SaveProfileHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            testRuns = mr.Create<IProfileRepository>();
            sut = new SaveProfileHandler(testRuns.Object);
        }

        [Fact]
        public async Task ShouldNotifyOnUpdatedTestRun()
        {
            var penalties = new[] { new Penalty(Domain.Enums.PenaltyEnum.Late, 1) };
            var profile = new Profile("a", "", "", Domain.Enums.Age.Junior);
            testRuns.Setup(a => a.Upsert(Its.EquivalentTo(profile), CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new("aa@aa.com", profile), CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
