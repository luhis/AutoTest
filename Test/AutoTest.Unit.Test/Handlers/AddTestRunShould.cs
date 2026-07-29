using System;
using System.Collections.Generic;
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
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<ITestRunsRepository> _testRunsRepository;
    private readonly Mock<IEventNotifier> _notifier;
    private readonly Mock<IMarshalsRepository> _marshalsRepository;
    private readonly Mock<IEventsRepository> _eventsRepository;
    private readonly IRequestHandler<AddTestRun, OneOf<Success, Error<string>>> _sut;

    private readonly ICollection<Penalty> _penalties = new[] { new Penalty(Domain.Enums.PenaltyEnum.Late, 1) };

    public AddTestRunShould()
    {
        _testRunsRepository = _mr.Create<ITestRunsRepository>();
        _notifier = _mr.Create<IEventNotifier>();
        _marshalsRepository = _mr.Create<IMarshalsRepository>();
        _eventsRepository = _mr.Create<IEventsRepository>();
        _sut = new AddTestRunHandler(_testRunsRepository.Object, _notifier.Object, _marshalsRepository.Object, _eventsRepository.Object);
    }

    [Fact]
    public async Task ShouldNotifyOnAddedTestRun()
    {
        var entrantId = 5ul;
        var marshalId = 6ul;
        var eventId = 1ul;
        var clubId = 2ul;
        _marshalsRepository.Setup(a => a.GetMarshalIdByEmail(eventId, "marshal@email.com", TestContext.Current.CancellationToken)).ReturnsAsync(marshalId).Verifiable(Times.Once);
        var @event = Models.GetEvent(eventId, clubId);
        @event.SetEventStatus(Domain.Enums.EventStatus.Running);
        _eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(@event).Verifiable(Times.Once);
        _notifier.Setup(a => a.NewTestRun(It.Is<TestRun>(r => r.EventId == eventId && r.Ordinal == 3), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);
        _testRunsRepository.Setup(a => a.AddTestRun(It.Is<TestRun>(r => r.EventId == eventId && r.Ordinal == 3), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);

        var res = await _sut.Handle(new(1, eventId, 3, 4, entrantId, new DateTime(2000, 1, 1), "marshal@email.com", _penalties), TestContext.Current.CancellationToken);

        res.AsT0.Should().NotBeNull();
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ShouldNotAddTestRunWhenNotRunning()
    {
        var entrantId = 5ul;
        var eventId = 1ul;
        var clubId = 2ul;
        var @event = Models.GetEvent(eventId, clubId);
        _eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(@event).Verifiable(Times.Once);

        var res = await _sut.Handle(new(1, eventId, 3, 4, entrantId, new DateTime(2000, 1, 1), "marshal@email.com", _penalties), TestContext.Current.CancellationToken);

        res.AsT1.Value.Should().Be("Event must be running to add Test Run");
        _mr.VerifyAll();
    }
}
