using System;
using System.Threading;
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
    private readonly IRequestHandler<SaveEntrant, OneOf<Entrant, Error<string>>> sut;
    private readonly MockRepository mr;
    private readonly Mock<IEntrantsRepository> entrantsRepository;
    private readonly Mock<IEventsRepository> eventsRepository;
    private readonly Mock<IAuthorisationNotifier> authorisationNotifier;

    public SaveEntrantHandlerShould()
    {
        mr = new MockRepository(MockBehavior.Strict);
        entrantsRepository = mr.Create<IEntrantsRepository>();
        eventsRepository = mr.Create<IEventsRepository>();
        authorisationNotifier = mr.Create<IAuthorisationNotifier>();
        sut = new SaveEntrantHandler(entrantsRepository.Object, eventsRepository.Object, authorisationNotifier.Object);
    }

    static Event GetEvent(ulong eventId, DateTime open, DateTime close) =>
        new Event(eventId, 1, "", DateTime.UtcNow, 3, 2, "", new[] { EventType.AutoTest }, "", TimingSystem.StopWatch, open, close, 10, new DateTime());

    [Theory]
    [InlineData(1, 2, "Please wait until event open")]
    [InlineData(-2, -1, "Event is now closed")]
    public async Task ErrorWhenEntryTimingInvalid(int openOffsetDays, int closeOffsetDays, string expectedError)
    {
        var entrantId = 1ul;
        var eventId = 2ul;
        var entrant = Models.GetEntrant(entrantId, eventId);
        entrant.SetPayment(new Payment());

        eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(GetEvent(eventId, DateTime.UtcNow.AddDays(openOffsetDays), DateTime.UtcNow.AddDays(closeOffsetDays)));

        var se = new SaveEntrant(entrant);
        var res = await sut.Handle(se, TestContext.Current.CancellationToken);

        res.AsT1.Value.Should().Be(expectedError);
        mr.VerifyAll();
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

        entrantsRepository.Setup(a => a.GetById(eventId, entrantId, TestContext.Current.CancellationToken)).ReturnsAsync(entrantFromDb);
        entrantsRepository.Setup(a => a.Upsert(entrant, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);
        eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(GetEvent(eventId, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(2)));
        entrantsRepository.Setup(a => a.GetEntrantCount(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(0);
        authorisationNotifier.Setup(a => a.AddEditableEntrant(entrantId, Its.EquivalentTo(new[] { "a@a.com" }), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        var se = new SaveEntrant(entrant);
        var res = await sut.Handle(se, TestContext.Current.CancellationToken);

        mr.VerifyAll();
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

        eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(GetEvent(eventId, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(1)));
        entrantsRepository.Setup(a => a.GetEntrantCount(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(10);
        entrantsRepository.Setup(a => a.GetById(eventId, entrantId, TestContext.Current.CancellationToken)).ReturnsAsync((Entrant?)null);
        entrantsRepository.Setup(a => a.Upsert(entrant, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);
        authorisationNotifier.Setup(a => a.AddEditableEntrant(entrantId, new[] { "a@a.com" }, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        var se = new SaveEntrant(entrant);
        var res = await sut.Handle(se, TestContext.Current.CancellationToken);

        res.AsT0.Should().Be(entrant);
        mr.VerifyAll();
    }
}
