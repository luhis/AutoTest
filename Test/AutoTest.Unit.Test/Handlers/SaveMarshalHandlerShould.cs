using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using AwesomeAssertions;
using AwesomeAssertions.ArgumentMatchers.Moq;
using Mediator;
using Moq;
using OneOf;
using OneOf.Types;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class SaveMarshalHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IAuthorisationNotifier> _authorisationNotifier;
    private readonly Mock<IMarshalsRepository> _marshalRepository;
    private readonly Mock<IEventsRepository> _eventsRepository;
    private readonly IRequestHandler<SaveMarshal, OneOf<Marshal, Error<string>>> _sut;

    public SaveMarshalHandlerShould()
    {
        _authorisationNotifier = _mr.Create<IAuthorisationNotifier>();
        _marshalRepository = _mr.Create<IMarshalsRepository>();
        _eventsRepository = _mr.Create<IEventsRepository>();
        _sut = new SaveMarshalHandler(_marshalRepository.Object, _eventsRepository.Object, _authorisationNotifier.Object);
    }

    static Event GetEvent(ulong eventId) =>
        new Event(eventId, 1, "", DateTime.UtcNow, 3, 2, new[] { EventType.AutoTest }, TimingSystem.StopWatch, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1), 10, new DateTime());

    [Fact]
    public async Task SaveMarshalExisting()
    {
        var marshalId = 1ul;
        var eventId = 2ul;
        var marshal = new Marshal(marshalId, "name", "familyName", "a@a.com", eventId, 123456, "");

        _eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(GetEvent(eventId)).Verifiable(Times.Once);
        _marshalRepository.Setup(a => a.GetById(eventId, marshalId, TestContext.Current.CancellationToken)).ReturnsAsync(marshal).Verifiable(Times.Once);
        _marshalRepository.Setup(a => a.Upsert(marshal, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);
        _authorisationNotifier.Setup(a => a.AddEditableMarshal(marshalId, Its.EquivalentTo(new[] { "a@a.com" }), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);

        var se = new SaveMarshal(marshal);
        var res = await _sut.Handle(se, TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }

    [Fact]
    public async Task SaveMarshalExistingUpdateEmail()
    {
        var marshalId = 1ul;
        var eventId = 2ul;
        var marshal = new Marshal(marshalId, "name", "familyName", "a@a.com", eventId, 123456, "");
        var marshal2 = new Marshal(marshalId, "name", "familyName", "b@a.com", eventId, 123456, "");

        _eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(GetEvent(eventId)).Verifiable(Times.Once);
        _marshalRepository.Setup(a => a.GetById(eventId, marshalId, TestContext.Current.CancellationToken)).ReturnsAsync(marshal).Verifiable(Times.Once);
        _marshalRepository.Setup(a => a.Upsert(marshal2, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);
        _authorisationNotifier.Setup(a => a.AddEditableMarshal(marshalId, Its.EquivalentTo(new[] { "b@a.com" }), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);
        _authorisationNotifier.Setup(a => a.RemoveEventMarshal(eventId, Its.EquivalentTo(new[] { "a@a.com" }), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);
        _authorisationNotifier.Setup(a => a.NewEventMarshal(eventId, Its.EquivalentTo(new[] { "b@a.com" }), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);

        var se = new SaveMarshal(marshal2);
        var res = await _sut.Handle(se, TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }

    [Fact]
    public async Task SaveMarshalNew()
    {
        var marshalId = 1ul;
        var eventId = 2ul;
        var marshal = new Marshal(marshalId, "name", "familyName", "a@a.com", eventId, 123456, "");

        _eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(GetEvent(eventId)).Verifiable(Times.Once);
        _marshalRepository.Setup(a => a.GetById(eventId, marshalId, TestContext.Current.CancellationToken)).ReturnsAsync((Marshal?)null).Verifiable(Times.Once);
        _marshalRepository.Setup(a => a.Upsert(marshal, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);
        _authorisationNotifier.Setup(a => a.NewEventMarshal(eventId, Its.EquivalentTo<IEnumerable<string>>(new[] { "a@a.com" }), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);
        _authorisationNotifier.Setup(a => a.AddEditableMarshal(marshalId, Its.EquivalentTo(new[] { "a@a.com" }), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);

        var se = new SaveMarshal(marshal);
        var res = await _sut.Handle(se, TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }

    [Fact]
    public async Task ErrorWhenEventCancelled()
    {
        var marshalId = 1ul;
        var eventId = 2ul;
        var marshal = new Marshal(marshalId, "name", "familyName", "a@a.com", eventId, 123456, "");

        var @event = GetEvent(eventId);
        @event.SetEventStatus(EventStatus.Cancelled);
        _eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(@event).Verifiable(Times.Once);

        var se = new SaveMarshal(marshal);
        var res = await _sut.Handle(se, TestContext.Current.CancellationToken);

        res.AsT1.Value.Should().Be("Event is cancelled");
        _mr.VerifyAll();
    }
}
