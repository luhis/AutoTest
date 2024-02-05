using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
using FluentAssertions;
using FluentAssertions.ArgumentMatchers.Moq;
using MediatR;
using Moq;
using OneOf.Types;
using OneOf;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class AddTestRunShould
    {
        private readonly IRequestHandler<AddTestRun, OneOf<Success, Error<string>>> sut;
        private readonly MockRepository mr;
        private readonly Mock<IEventNotifier> notifier;
        private readonly Mock<ITestRunsRepository> testRuns;
        private readonly Mock<IEventsRepository> events;
        private readonly Mock<IMarshalsRepository> marshalsRepository;
        private readonly ICollection<Penalty> penalties = new[] { new Penalty(Domain.Enums.PenaltyEnum.Late, 1) };

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
            var tr = new TestRun(1, eventId, 3, 4, entrantId, new DateTime(2000, 1, 1), marshalId);
            tr.SetPenalties(penalties);
            marshalsRepository.Setup(a => a.GetMarshalIdByEmail(eventId, "marshal@email.com", CancellationToken.None)).ReturnsAsync(marshalId);
            var @event = Models.GetEvent(eventId, clubId);
            @event.SetEventStatus(Domain.Enums.EventStatus.Running);
            events.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(@event);

            notifier.Setup(a => a.NewTestRun(Its.EquivalentTo(tr), CancellationToken.None)).Returns(Task.CompletedTask);
            testRuns.Setup(a => a.AddTestRun(Its.EquivalentTo(tr), CancellationToken.None)).Returns(Task.CompletedTask);

            var res = await sut.Handle(new(1, eventId, 3, 4, entrantId, new DateTime(2000, 1, 1), "marshal@email.com", penalties), CancellationToken.None);

            res.AsT0.Should().NotBeNull();
            mr.VerifyAll();
        }

        [Fact]
        public async Task ShouldNotAddTestRunWhenNotRunning()
        {
            var entrantId = 5ul;
            var marshalId = 6ul;
            var eventId = 1ul;
            var clubId = 2ul;
            var tr = new TestRun(1, eventId, 3, 4, entrantId, new DateTime(2000, 1, 1), marshalId);
            tr.SetPenalties(penalties);
            var @event = Models.GetEvent(eventId, clubId);
            events.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(@event);

            var res = await sut.Handle(new(1, eventId, 3, 4, entrantId, new DateTime(2000, 1, 1), "marshal@email.com", penalties), CancellationToken.None);
            res.AsT1.Value.Should().Be("Event must be running to add Test Run");

            mr.VerifyAll();
        }
    }
}
