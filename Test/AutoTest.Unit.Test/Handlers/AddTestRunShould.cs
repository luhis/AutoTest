using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.Fixtures;
using FluentAssertions.ArgumentMatchers.Moq;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class AddTestRunShould
    {
        private readonly IRequestHandler<AddTestRun, MediatR.Unit> sut;
        private readonly MockRepository mr;
        private readonly Mock<ISignalRNotifier> notifier;
        private readonly Mock<ITestRunsRepository> testRuns;
        private readonly AutoTestContext context;

        public AddTestRunShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            notifier = mr.Create<ISignalRNotifier>();
            testRuns = mr.Create<ITestRunsRepository>();
            context = InMemDbFixture.GetDbContext();
            sut = new AddTestRunHandler(testRuns.Object, notifier.Object, context);
        }

        [Fact]
        public async Task ShouldNotifyOnAddedTestRun()
        {
            var entrantId = 5ul;
            var marshalId = 6ul;
            var eventId = 1ul;
            var clubId = 2ul;
            var penalties = new[] { new Penalty(Domain.Enums.PenaltyEnum.Late, 1) };
            var tr = new TestRun(1, eventId, 3, 4, entrantId, new System.DateTime(2000, 1, 1), marshalId);
            tr.SetPenalties(penalties);
            context.Marshals!.Add(new(marshalId, "dave", "marshal", "marshal@email.com", eventId, 12345, "marshal"));
            context.Events!.Add(new(eventId, clubId, "location", new System.DateTime(2000, 1, 1), 2, 3, "regs", Domain.Enums.EventType.AutoSolo, "maps", Domain.Enums.TimingSystem.App));
            context.Clubs!.Add(new(clubId, "club", "pay@paypal.com", "www.club.com"));
            await context.SaveChangesAsync();
            notifier.Setup(a => a.NewTestRun(Its.EquivalentTo(tr), CancellationToken.None)).Returns(Task.CompletedTask);
            testRuns.Setup(a => a.AddTestRun(Its.EquivalentTo(tr), CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new(1, eventId, 3, 4, entrantId, new System.DateTime(2000, 1, 1), "marshal@email.com", penalties), CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
