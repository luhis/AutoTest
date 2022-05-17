
using System.Threading;
using System.Threading.Tasks;
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

        public SaveEntrantHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            entrantsRepository = mr.Create<IEntrantsRepository>();
            sut = new SaveEntrantHandler(entrantsRepository.Object);
        }

        [Fact]
        public async Task NotOverwritePaymentMethodWhenNone()
        {
            var entrantId = 1ul;
            var eventId = 2ul;
            var entrant = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", eventId, "BRMC", 123456, Domain.Enums.Age.Senior);
            entrant.SetPayment(new Payment());

            var entrantFromDb = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", eventId, "BRMC", 123456, Domain.Enums.Age.Senior);
            entrantsRepository.Setup(a => a.GetById(eventId, entrantId, CancellationToken.None)).ReturnsAsync(entrantFromDb);
            entrantsRepository.Setup(a => a.Upsert(entrant, CancellationToken.None)).Returns(Task.CompletedTask);
            var se = new SaveEntrant(entrant);
            var res = await sut.Handle(se, CancellationToken.None);

            mr.VerifyAll();
            res.Payment.Should().BeNull();
        }

        [Fact]
        public async Task NotOverwritePaymentMethodWhenSome()
        {
            var entrantId = 1ul;
            var eventId = 2ul;
            var entrant = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", eventId, "BRMC", 123456, Domain.Enums.Age.Senior);

            var entrantFromDb = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", eventId, "BRMC", 123456, Domain.Enums.Age.Senior);
            entrantFromDb.SetPayment(new Payment());
            entrantsRepository.Setup(a => a.GetById(eventId, entrantId, CancellationToken.None)).ReturnsAsync(entrantFromDb);
            entrantsRepository.Setup(a => a.Upsert(entrant, CancellationToken.None)).Returns(Task.CompletedTask);
            var se = new SaveEntrant(entrant);
            var res = await sut.Handle(se, CancellationToken.None);

            mr.VerifyAll();
            res.Payment.Should().NotBeNull();
        }
    }
}
