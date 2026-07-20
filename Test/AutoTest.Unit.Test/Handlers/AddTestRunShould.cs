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
using AwesomeAssertions;
using Mediator;
using Moq;
using OneOf;
using OneOf.Types;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class AddTestRunShould
{
    private readonly IRequestHandler<AddTestRun, OneOf<Success, Error<string>>> sut;
    private readonly MockRepository mr;
    private readonly Mock<ITestRunsRepository> testRunsRepository;
    private readonly Mock<IEventNotifier> notifier;
    private readonly Mock<IMarshalsRepository> marshalsRepository;
    private readonly Mock<IEventsRepository> eventsRepository;

    private readonly ICollection<Penalty> penalties = new[] { new Penalty(Domain.Enums.PenaltyEnum.Late, 1) };

    public AddTestRunShould()
    {
        mr = new MockRepository(MockBehavior.Strict);
        testRunsRepository = mr.Create<ITestRunsRepository>();
        notifier = mr.Create<IEventNotifier>();
        marshalsRepository = mr.Create<IMarshalsRepository>();
        eventsRepository = mr.Create<IEventsRepository>();
        sut = new AddTestRunHandler(testRunsRepository.Object, notifier.Object, marshalsRepository.Object, eventsRepository.Object);
    }

    [Fact]
    public async Task ShouldNotifyOnAddedTestRun()
    {
        var entrantId = 5ul;
        var marshalId = 6ul;
        var eventId = 1ul;
        var clubId = 2ul;
        marshalsRepository.Setup(a => a.GetMarshalIdByEmail(eventId, "marshal@email.com", CancellationToken.None)).ReturnsAsync(marshalId);
        var @event = Models.GetEvent(eventId, clubId);
        @event.SetEventStatus(Domain.Enums.EventStatus.Running);
        eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(@event);
        notifier.Setup(a => a.NewTestRun(It.Is<TestRun>(r => r.EventId == eventId && r.Ordinal == 3), CancellationToken.None)).Returns(Task.CompletedTask);
        testRunsRepository.Setup(a => a.AddTestRun(It.Is<TestRun>(r => r.EventId == eventId && r.Ordinal == 3), CancellationToken.None)).Returns(Task.CompletedTask);

        var res = await sut.Handle(new(1, eventId, 3, 4, entrantId, new DateTime(2000, 1, 1), "marshal@email.com", penalties), CancellationToken.None);

        res.AsT0.Should().NotBeNull();
        mr.VerifyAll();
    }

    [Fact]
    public async Task ShouldNotAddTestRunWhenNotRunning()
    {
        var entrantId = 5ul;
        var eventId = 1ul;
        var clubId = 2ul;
        var @event = Models.GetEvent(eventId, clubId);
        eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(@event);

        var res = await sut.Handle(new(1, eventId, 3, 4, entrantId, new DateTime(2000, 1, 1), "marshal@email.com", penalties), CancellationToken.None);

        res.AsT1.Value.Should().Be("Event must be running to add Test Run");
        mr.VerifyAll();
    }
}
