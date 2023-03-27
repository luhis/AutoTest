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
using System;
using FluentAssertions;

namespace AutoTest.Unit.Test.Handlers
{
    public class AddTestRunShould
    {
        private readonly IRequestHandler<AddTestRun> sut;
        private readonly MockRepository mr;
        private readonly Mock<IEventNotifier> notifier;
        private readonly Mock<ITestRunsRepository> testRuns;
        private readonly Mock<IEventsRepository> events;
        private readonly Mock<IMarshalsRepository> marshalsRepository;

        public AddTestRunShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            notifier = mr.Create<IEventNotifier>();
            testRuns = mr.Create<ITestRunsRepository>();
            events = mr.Create<IEventsRepository>();
            marshalsRepository = mr.Create<IMarshalsRepository>();
            sut = new AddTestRunHandler(testRuns.Object, notifier.Object, marshalsRepository.Object, events.Object);
        }

        [Fact]
        public async Task ShouldNotifyOnAddedTestRun()
        {
            var entrantId = 5ul;
            var marshalId = 6ul;
            var eventId = 1ul;
            var clubId = 2ul;
            var penalties = new[] { new Penalty(Domain.Enums.PenaltyEnum.Late, 1) };
            var tr = new TestRun(1, eventId, 3, 4, entrantId, new DateTime(2000, 1, 1), marshalId);
            tr.SetPenalties(penalties);
            marshalsRepository.Setup(a => a.GetMashalIdByEmail(eventId, "marshal@email.com", CancellationToken.None)).ReturnsAsync(marshalId);
            var @event = new Event(eventId, clubId, "location", new DateTime(2000, 1, 1), 2, 3, "regs", new[] { Domain.Enums.EventType.AutoSolo }, "maps", Domain.Enums.TimingSystem.App, new DateTime(), new DateTime(), 10);
            @event.SetEventStatus(Domain.Enums.EventStatus.Running);
            events.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(@event);

            notifier.Setup(a => a.NewTestRun(Its.EquivalentTo(tr), CancellationToken.None)).Returns(Task.CompletedTask);
            testRuns.Setup(a => a.AddTestRun(Its.EquivalentTo(tr), CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new(1, eventId, 3, 4, entrantId, new DateTime(2000, 1, 1), "marshal@email.com", penalties), CancellationToken.None);

            mr.VerifyAll();
        }

        [Fact]
        public async Task ShouldNotAddTestRunWhenNotRunning()
        {
            var entrantId = 5ul;
            var marshalId = 6ul;
            var eventId = 1ul;
            var clubId = 2ul;
            var penalties = new[] { new Penalty(Domain.Enums.PenaltyEnum.Late, 1) };
            var tr = new TestRun(1, eventId, 3, 4, entrantId, new DateTime(2000, 1, 1), marshalId);
            tr.SetPenalties(penalties);
            var @event = new Event(eventId, clubId, "location", new DateTime(2000, 1, 1), 2, 3, "regs", new[] { Domain.Enums.EventType.AutoSolo }, "maps", Domain.Enums.TimingSystem.App, new DateTime(), new DateTime(), 10);
            events.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(@event);

            Func<Task> act = () => sut.Handle(new(1, eventId, 3, 4, entrantId, new DateTime(2000, 1, 1), "marshal@email.com", penalties), CancellationToken.None);
            await act.Should().ThrowAsync<Exception>().WithMessage("Event must be running to add Test Run");

            mr.VerifyAll();
        }
    }
}
