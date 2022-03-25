using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class UpdateTestRunShould
    {
        private readonly IRequestHandler<UpdateTestRun, MediatR.Unit> sut;
        private readonly MockRepository mr;
        private readonly Mock<ISignalRNotifier> notifier;
        private readonly Mock<ITestRunsRepository> testRuns;

        public UpdateTestRunShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            notifier = mr.Create<ISignalRNotifier>();
            testRuns = mr.Create<ITestRunsRepository>();
            sut = new UpdateTestRunHandler(testRuns.Object, notifier.Object);
        }

        [Fact]
        public async Task ShouldNotifyOnNewNotification()
        {
            var entrantId = 5ul;
            var marshalId = 6ul;
            var tr = new TestRun(1, 2, 3, 4, entrantId, new System.DateTime(), marshalId);
            var notification = new Notification(1, 2, "message", new System.DateTime(2000, 1, 1), "test user");
            // todo more validation
            notifier.Setup(a => a.NewTestRun(It.Is<TestRun>(a => a.EntrantId == entrantId), CancellationToken.None)).Returns(Task.CompletedTask);
            testRuns.Setup(a => a.UpdateTestRun(It.Is<TestRun>(a => a.EntrantId == entrantId), CancellationToken.None)).Returns(Task.CompletedTask);

            var res = await sut.Handle(new(1, 2, 3, 4, entrantId, new System.DateTime(2000, 1, 1), marshalId, new[] { new Penalty(Domain.Enums.PenaltyEnum.Late, 1) }), CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
