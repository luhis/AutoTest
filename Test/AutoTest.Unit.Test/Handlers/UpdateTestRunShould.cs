using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using FluentAssertions.ArgumentMatchers.Moq;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class UpdateTestRunShould
    {
        private readonly IRequestHandler<UpdateTestRun> sut;
        private readonly MockRepository mr;
        private readonly Mock<IEventNotifier> notifier;
        private readonly Mock<ITestRunsRepository> testRuns;

        public UpdateTestRunShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            notifier = mr.Create<IEventNotifier>();
            testRuns = mr.Create<ITestRunsRepository>();
            sut = new UpdateTestRunHandler(testRuns.Object, notifier.Object);
        }

        [Fact]
        public async Task ShouldNotifyOnUpdatedTestRun()
        {
            var entrantId = 5ul;
            var marshalId = 6ul;
            var penalties = new[] { new Penalty(Domain.Enums.PenaltyEnum.Late, 1) };
            var tr = new TestRun(1, 2, 3, 4, entrantId, new System.DateTime(2000, 1, 1), marshalId);
            tr.SetPenalties(penalties);
            notifier.Setup(a => a.NewTestRun(Its.EquivalentTo(tr), CancellationToken.None)).Returns(Task.CompletedTask);
            testRuns.Setup(a => a.UpdateTestRun(Its.EquivalentTo(tr), CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new(1, 2, 3, 4, entrantId, new System.DateTime(2000, 1, 1), marshalId, penalties), CancellationToken.None);

            mr.VerifyAll();
        }

        [Fact(Skip = "Todo not implemented")]
        public async Task NotChangeCreatedDate()
        {
            var entrantId = 5ul;
            var marshalId = 6ul;
            var penalties = new[] { new Penalty(Domain.Enums.PenaltyEnum.Late, 1) };
            var tr = new TestRun(1, 2, 3, 4, entrantId, new System.DateTime(2000, 1, 1), marshalId);
            tr.SetPenalties(penalties);
            notifier.Setup(a => a.NewTestRun(Its.EquivalentTo(tr), CancellationToken.None)).Returns(Task.CompletedTask);
            testRuns.Setup(a => a.UpdateTestRun(Its.EquivalentTo(tr), CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new(1, 2, 3, 4, entrantId, new System.DateTime(2000, 1, 2), marshalId, penalties), CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
