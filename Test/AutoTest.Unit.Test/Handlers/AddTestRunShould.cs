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
using Moq.AutoMock;
using OneOf;
using OneOf.Types;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class AddTestRunShould
    {
        private readonly ICollection<Penalty> penalties = new[] { new Penalty(Domain.Enums.PenaltyEnum.Late, 1) };

        [Fact]
        public async Task ShouldNotifyOnAddedTestRun()
        {
            var mocker = new AutoMocker(MockBehavior.Strict);
            var entrantId = 5ul;
            var marshalId = 6ul;
            var eventId = 1ul;
            var clubId = 2ul;
            var tr = new TestRun(1, eventId, 3, 4, entrantId, new DateTime(2000, 1, 1), marshalId);
            tr.SetPenalties(penalties);
            var marshalsRepository = mocker.GetMock<IMarshalsRepository>();
            marshalsRepository.Setup(a => a.GetMarshalIdByEmail(eventId, "marshal@email.com", CancellationToken.None)).ReturnsAsync(marshalId);
            var @event = Models.GetEvent(eventId, clubId);
            @event.SetEventStatus(Domain.Enums.EventStatus.Running);
            var events = mocker.GetMock<IEventsRepository>();
            events.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(@event);

            var notifier = mocker.GetMock<IEventNotifier>();
            notifier.Setup(a => a.NewTestRun(Its.EquivalentTo(tr), CancellationToken.None)).Returns(Task.CompletedTask);
            var testRuns = mocker.GetMock<ITestRunsRepository>();
            testRuns.Setup(a => a.AddTestRun(Its.EquivalentTo(tr), CancellationToken.None)).Returns(Task.CompletedTask);

            IRequestHandler<AddTestRun, OneOf<Success, Error<string>>> sut = mocker.CreateInstance<AddTestRunHandler>();
            var res = await sut.Handle(new(1, eventId, 3, 4, entrantId, new DateTime(2000, 1, 1), "marshal@email.com", penalties), CancellationToken.None);

            res.AsT0.Should().NotBeNull();
            mocker.VerifyAll();
        }

        [Fact]
        public async Task ShouldNotAddTestRunWhenNotRunning()
        {
            var mocker = new AutoMocker(MockBehavior.Strict);
            var entrantId = 5ul;
            var marshalId = 6ul;
            var eventId = 1ul;
            var clubId = 2ul;
            var tr = new TestRun(1, eventId, 3, 4, entrantId, new DateTime(2000, 1, 1), marshalId);
            tr.SetPenalties(penalties);
            var @event = Models.GetEvent(eventId, clubId);
            var events = mocker.GetMock<IEventsRepository>();
            events.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(@event);

            IRequestHandler<AddTestRun, OneOf<Success, Error<string>>> sut = mocker.CreateInstance<AddTestRunHandler>();
            var res = await sut.Handle(new(1, eventId, 3, 4, entrantId, new DateTime(2000, 1, 1), "marshal@email.com", penalties), CancellationToken.None);

            res.AsT1.Value.Should().Be("Event must be running to add Test Run");
            mocker.VerifyAll();
        }
    }
}
