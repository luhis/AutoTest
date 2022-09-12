
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class SaveEntrantHandlerShould
    {
        private readonly IRequestHandler<SaveEntrant, Entrant> sut;
        private readonly MockRepository mr;
        private readonly Mock<IEntrantsRepository> entrantsRepository;
        private readonly Mock<IEventsRepository> eventsRepository;

        public SaveEntrantHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            entrantsRepository = mr.Create<IEntrantsRepository>();
            eventsRepository = mr.Create<IEventsRepository>();
            sut = new SaveEntrantHandler(entrantsRepository.Object, eventsRepository.Object);
        }

        [Fact]
        public async Task NotOverwritePaymentMethodWhenNone()
        {
            var entrantId = 1ul;
            var eventId = 2ul;
            var entrant = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", eventId, "BRMC", 123456, Age.Senior);
            entrant.SetPayment(new Payment());

            var entrantFromDb = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", eventId, "BRMC", 123456, Age.Senior);
            entrantsRepository.Setup(a => a.GetById(eventId, entrantId, CancellationToken.None)).ReturnsAsync(entrantFromDb);
            entrantsRepository.Setup(a => a.Upsert(entrant, CancellationToken.None)).Returns(Task.CompletedTask);
            eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(new Event(eventId, 1, "", DateTime.UtcNow, 3, 2, "", new[] { EventType.AutoTest }, "", TimingSystem.StopWatch, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(2), 10));
            entrantsRepository.Setup(a => a.GetEntrantCount(eventId, CancellationToken.None)).ReturnsAsync(0);

            var se = new SaveEntrant(entrant);
            var res = await sut.Handle(se, CancellationToken.None);

            mr.VerifyAll();
            res.Payment.Should().BeNull();
        }

        [Fact]
        public async Task ErrorWhenBeforeEntryStartAsync()
        {
            var entrantId = 1ul;
            var eventId = 2ul;
            var entrant = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", eventId, "BRMC", 123456, Age.Senior);
            entrant.SetPayment(new Payment());

            eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(new Event(eventId, 1, "", DateTime.UtcNow, 3, 2, "", new[] { EventType.AutoTest }, "", TimingSystem.StopWatch, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2), 10));

            var se = new SaveEntrant(entrant);
            Func<Task> act = () => sut.Handle(se, CancellationToken.None);
            var exception = await act.Should().ThrowAsync<Exception>();
            exception.WithMessage("Please wait until event open");

            mr.VerifyAll();
        }

        [Fact]
        public async Task ErrorWhenAfterEntryCloseAsync()
        {
            var entrantId = 1ul;
            var eventId = 2ul;
            var entrant = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", eventId, "BRMC", 123456, Age.Senior);
            entrant.SetPayment(new Payment());

            eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(new Event(eventId, 1, "", DateTime.UtcNow, 3, 2, "", new[] { EventType.AutoTest }, "", TimingSystem.StopWatch, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(-1), 10));

            var se = new SaveEntrant(entrant);
            Func<Task> act = () => sut.Handle(se, CancellationToken.None);
            var exception = await act.Should().ThrowAsync<Exception>();
            exception.WithMessage("Event is now closed");

            mr.VerifyAll();
        }

        [Fact]
        public async Task ErrorWhenTooManyEntrantsAsync()
        {
            var entrantId = 1ul;
            var eventId = 2ul;
            var entrant = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", eventId, "BRMC", 123456, Age.Senior);
            entrant.SetPayment(new Payment());

            eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(new Event(eventId, 1, "", DateTime.UtcNow, 3, 2, "", new[] { EventType.AutoTest }, "", TimingSystem.StopWatch, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(1), 10));
            entrantsRepository.Setup(a => a.GetEntrantCount(eventId, CancellationToken.None)).ReturnsAsync(10);

            var se = new SaveEntrant(entrant);
            Func<Task> act = () => sut.Handle(se, CancellationToken.None);
            var exception = await act.Should().ThrowAsync<Exception>();
            exception.WithMessage("Too many entrants");

            mr.VerifyAll();
        }

        [Fact]
        public async Task NotOverwritePaymentMethodWhenSome()
        {
            var entrantId = 1ul;
            var eventId = 2ul;
            var entrant = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", eventId, "BRMC", 123456, Age.Senior);

            var entrantFromDb = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", eventId, "BRMC", 123456, Age.Senior);
            entrantFromDb.SetPayment(new Payment());
            entrantsRepository.Setup(a => a.GetById(eventId, entrantId, CancellationToken.None)).ReturnsAsync(entrantFromDb);
            entrantsRepository.Setup(a => a.Upsert(entrant, CancellationToken.None)).Returns(Task.CompletedTask);
            eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(new Event(eventId, 1, "", DateTime.UtcNow, 3, 2, "", new[] { EventType.AutoTest }, "", TimingSystem.StopWatch, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(2), 10));
            entrantsRepository.Setup(a => a.GetEntrantCount(eventId, CancellationToken.None)).ReturnsAsync(0);

            var se = new SaveEntrant(entrant);
            var res = await sut.Handle(se, CancellationToken.None);

            mr.VerifyAll();
            res.Payment.Should().NotBeNull();
        }
    }
}
