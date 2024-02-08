using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using FluentAssertions;
using FluentAssertions.ArgumentMatchers.Moq;
using MediatR;
using Moq;
using OneOf;
using OneOf.Types;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
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

        [Fact]
        public async Task NotOverwritePaymentMethodWhenNone()
        {
            var entrantId = 1ul;
            var eventId = 2ul;
            var entrant = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", EventType.AutoTest, "A", eventId, "BRMC", "123456", Age.Senior, false);
            entrant.SetPayment(new Payment());

            var entrantFromDb = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", EventType.AutoTest, "A", eventId, "BRMC", "123456", Age.Senior, false);
            entrantsRepository.Setup(a => a.GetById(eventId, entrantId, CancellationToken.None)).ReturnsAsync(entrantFromDb);
            entrantsRepository.Setup(a => a.Upsert(entrant, CancellationToken.None)).Returns(Task.CompletedTask);
            eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(GetEvent(eventId, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(2)));
            entrantsRepository.Setup(a => a.GetEntrantCount(eventId, CancellationToken.None)).ReturnsAsync(0);
            authorisationNotifier.Setup(a => a.AddEditableEntrant(entrantId, Its.EquivalentTo(new[] { "a@a.com" }), CancellationToken.None)).Returns(Task.CompletedTask);

            var se = new SaveEntrant(entrant);
            var res = await sut.Handle(se, CancellationToken.None);

            mr.VerifyAll();
            res.AsT0.Payment.Should().BeNull();
        }

        [Fact]
        public async Task ErrorWhenBeforeEntryStartAsync()
        {
            var entrantId = 1ul;
            var eventId = 2ul;
            var entrant = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", EventType.AutoTest, "A", eventId, "BRMC", "123456", Age.Senior, false);
            entrant.SetPayment(new Payment());

            eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(GetEvent(eventId, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2)));

            var se = new SaveEntrant(entrant);
            var res = await sut.Handle(se, CancellationToken.None);

            res.AsT1.Value.Should().Be("Please wait until event open");
            mr.VerifyAll();
        }

        [Fact]
        public async Task ErrorWhenAfterEntryCloseAsync()
        {
            var entrantId = 1ul;
            var eventId = 2ul;
            var entrant = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", EventType.AutoTest, "A", eventId, "BRMC", "123456", Age.Senior, false);
            entrant.SetPayment(new Payment());

            eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(GetEvent(eventId, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(-1)));

            var se = new SaveEntrant(entrant);
            var res = await sut.Handle(se, CancellationToken.None);

            res.AsT1.Value.Should().Be("Event is now closed");
            mr.VerifyAll();
        }

        [Fact]
        public async Task ErrorWhenTooManyEntrantsAsync()
        {
            var entrantId = 1ul;
            var eventId = 2ul;
            var entrant = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", EventType.AutoTest, "A", eventId, "BRMC", "123456", Age.Senior, false);
            entrant.SetPayment(new Payment());

            eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(GetEvent(eventId, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(1)));
            entrantsRepository.Setup(a => a.GetEntrantCount(eventId, CancellationToken.None)).ReturnsAsync(10);
            entrantsRepository.Setup(a => a.GetById(eventId, entrantId, CancellationToken.None)).ReturnsAsync((Entrant?)null);
            entrantsRepository.Setup(a => a.Upsert(entrant, CancellationToken.None)).Returns(Task.CompletedTask);
            authorisationNotifier.Setup(a => a.AddEditableEntrant(entrantId, new[] { "a@a.com" }, CancellationToken.None)).Returns(Task.CompletedTask);

            var se = new SaveEntrant(entrant);
            var res = await sut.Handle(se, CancellationToken.None);

            res.AsT0.Should().Be(entrant);
            mr.VerifyAll();
        }

        [Fact]
        public async Task ErrorWhenWrongEventTypeAsync()
        {
            var entrantId = 1ul;
            var eventId = 2ul;
            var entrant = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", EventType.PCA, "A", eventId, "BRMC", "123456", Age.Senior, false);
            entrant.SetPayment(new Payment());

            eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(GetEvent(eventId, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(1)));
            entrantsRepository.Setup(a => a.GetEntrantCount(eventId, CancellationToken.None)).ReturnsAsync(1);

            var se = new SaveEntrant(entrant);
            var res = await sut.Handle(se, CancellationToken.None);

            res.AsT1.Value.Should().Be("Event Type invalid");
            mr.VerifyAll();
        }

        [Fact]
        public async Task NotOverwritePaymentMethodWhenSome()
        {
            var entrantId = 1ul;
            var eventId = 2ul;
            var entrant = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", EventType.AutoTest, "A", eventId, "BRMC", "123456", Age.Senior, false);

            var entrantFromDb = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", EventType.AutoTest, "A", eventId, "BRMC", "123456", Age.Senior, false);
            entrantFromDb.SetPayment(new Payment());
            entrantsRepository.Setup(a => a.GetById(eventId, entrantId, CancellationToken.None)).ReturnsAsync(entrantFromDb);
            entrantsRepository.Setup(a => a.Upsert(entrant, CancellationToken.None)).Returns(Task.CompletedTask);
            eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(GetEvent(eventId, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(2)));
            entrantsRepository.Setup(a => a.GetEntrantCount(eventId, CancellationToken.None)).ReturnsAsync(0);
            authorisationNotifier.Setup(a => a.AddEditableEntrant(entrantId, Its.EquivalentTo(new[] { "a@a.com" }), CancellationToken.None)).Returns(Task.CompletedTask);

            var se = new SaveEntrant(entrant);
            var res = await sut.Handle(se, CancellationToken.None);

            mr.VerifyAll();
            res.AsT0.Payment.Should().NotBeNull();
        }
    }
}
