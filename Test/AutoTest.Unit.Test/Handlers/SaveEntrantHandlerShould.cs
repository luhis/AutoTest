using System;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
using AwesomeAssertions;
using AwesomeAssertions.ArgumentMatchers.Moq;
using Mediator;
using Moq;
using OneOf;
using OneOf.Types;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class SaveEntrantHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IEntrantsRepository> _entrantsRepository;
    private readonly Mock<IEventsRepository> _eventsRepository;
    private readonly Mock<IAuthorisationNotifier> _authorisationNotifier;
    private readonly IRequestHandler<SaveEntrant, OneOf<Entrant, Error<string>>> _sut;

    public SaveEntrantHandlerShould()
    {
        _entrantsRepository = _mr.Create<IEntrantsRepository>();
        _eventsRepository = _mr.Create<IEventsRepository>();
        _authorisationNotifier = _mr.Create<IAuthorisationNotifier>();
        _sut = new SaveEntrantHandler(_entrantsRepository.Object, _eventsRepository.Object, _authorisationNotifier.Object);
    }

    static Event GetEvent(ulong eventId, DateTime open, DateTime close) =>
        new Event(eventId, 1, "", DateTime.UtcNow, 3, 2, new[] { EventType.AutoTest }, TimingSystem.StopWatch, open, close, 10, new DateTime());

    [Theory]
    [InlineData(1, 2, "Please wait until event open")]
    [InlineData(-2, -1, "Event is now closed")]
    public async Task ErrorWhenEntryTimingInvalid(int openOffsetDays, int closeOffsetDays, string expectedError)
    {
        var entrantId = 1ul;
        var eventId = 2ul;
        var entrant = Models.GetEntrant(entrantId, eventId);
        entrant.SetPayment(new Payment());

        _eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(GetEvent(eventId, DateTime.UtcNow.AddDays(openOffsetDays), DateTime.UtcNow.AddDays(closeOffsetDays))).Verifiable(Times.Once);

        var se = new SaveEntrant(entrant);
        var res = await _sut.Handle(se, TestContext.Current.CancellationToken);

        res.AsT1.Value.Should().Be(expectedError);
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ErrorWhenEventCancelled()
    {
        var entrantId = 1ul;
        var eventId = 2ul;
        var entrant = Models.GetEntrant(entrantId, eventId);
        entrant.SetPayment(new Payment());

        var @event = GetEvent(eventId, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(2));
        @event.SetEventStatus(EventStatus.Cancelled);
        _eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(@event).Verifiable(Times.Once);

        var se = new SaveEntrant(entrant);
        var res = await _sut.Handle(se, TestContext.Current.CancellationToken);

        res.AsT1.Value.Should().Be("Event is cancelled");
        _mr.VerifyAll();
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async Task NotOverwritePaymentMethod(bool entrantHasPayment, bool dbHasPayment)
    {
        var entrantId = 1ul;
        var eventId = 2ul;
        var entrant = Models.GetEntrant(entrantId, eventId);
        if (entrantHasPayment)
            entrant.SetPayment(new Payment());

        var entrantFromDb = Models.GetEntrant(entrantId, eventId);
        if (dbHasPayment)
            entrantFromDb.SetPayment(new Payment());

        _entrantsRepository.Setup(a => a.GetById(eventId, entrantId, TestContext.Current.CancellationToken)).ReturnsAsync(entrantFromDb).Verifiable(Times.Once);
        _entrantsRepository.Setup(a => a.Upsert(entrant, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);
        _eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(GetEvent(eventId, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(2))).Verifiable(Times.Once);
        _entrantsRepository.Setup(a => a.GetEntrantCount(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(0).Verifiable(Times.Once);
        _authorisationNotifier.Setup(a => a.AddEditableEntrant(entrantId, Its.EquivalentTo(new[] { "a@a.com" }), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);

        var se = new SaveEntrant(entrant);
        var res = await _sut.Handle(se, TestContext.Current.CancellationToken);

        _mr.VerifyAll();
        if (entrantHasPayment)
            res.AsT0.Payment.Should().BeNull();
        else
            res.AsT0.Payment.Should().NotBeNull();
    }

    [Fact]
    public async Task ErrorWhenTooManyEntrants()
    {
        var entrantId = 1ul;
        var eventId = 2ul;
        var entrant = Models.GetEntrant(entrantId, eventId);
        entrant.SetPayment(new Payment());

        _eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(GetEvent(eventId, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(1))).Verifiable(Times.Once);
        _entrantsRepository.Setup(a => a.GetEntrantCount(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(10).Verifiable(Times.Once);
        _entrantsRepository.Setup(a => a.GetById(eventId, entrantId, TestContext.Current.CancellationToken)).ReturnsAsync((Entrant?)null).Verifiable(Times.Once);
        _entrantsRepository.Setup(a => a.Upsert(entrant, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);
        _authorisationNotifier.Setup(a => a.AddEditableEntrant(entrantId, new[] { "a@a.com" }, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask).Verifiable(Times.Once);

        var se = new SaveEntrant(entrant);
        var res = await _sut.Handle(se, TestContext.Current.CancellationToken);

        res.AsT0.Should().Be(entrant);
        _mr.VerifyAll();
    }
}
