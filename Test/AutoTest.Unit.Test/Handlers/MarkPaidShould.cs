using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class MarkPaidShould
    {
        private readonly IRequestHandler<MarkPaid, MediatR.Unit> sut;
        private readonly MockRepository mr;
        private readonly Mock<IEntrantsRepository> entrantsRepository;

        public MarkPaidShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            entrantsRepository = mr.Create<IEntrantsRepository>();
            sut = new MarkPaidHandler(entrantsRepository.Object);
        }

        [Fact]
        public async Task MarkNotPaid()
        {
            var entrantId = 1ul;
            var eventId = 22ul;
            var entrant = new Entrant(entrantId, 1, "matt", "mccorry", "a@a.com", "A", eventId, "BRMC", 1234, Domain.Enums.Age.Senior);
            entrant.SetPayment(new(new System.DateTime(2000, 1, 1), Domain.Enums.PaymentMethod.Paypal, new System.DateTime(2000, 2, 2)));
            entrantsRepository.Setup(a => a.GetById(eventId, entrantId, CancellationToken.None)).Returns(Task.FromResult<Entrant?>(entrant));
            entrantsRepository.Setup(a => a.Update(entrant, CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new(eventId, entrantId, null), CancellationToken.None);

            mr.VerifyAll();
            entrantsRepository.Verify(a => a.Update(It.Is<Entrant>(a => a.Payment == null), CancellationToken.None));
        }

        [Fact]
        public async Task MarkPaid()
        {
            var entrantId = 1ul;
            var eventId = 22ul;
            var entrant = new Entrant(entrantId, 1, "matt", "mccorry", "a@a.com", "A", eventId, "BRMC", 1234, Domain.Enums.Age.Senior);
            entrantsRepository.Setup(a => a.GetById(eventId, entrantId, CancellationToken.None)).Returns(Task.FromResult<Entrant?>(entrant));
            entrantsRepository.Setup(a => a.Update(entrant, CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new(eventId, entrantId, new(new System.DateTime(2000, 1, 1), Domain.Enums.PaymentMethod.Paypal, new System.DateTime(2000, 2, 2))), CancellationToken.None);

            mr.VerifyAll();
            entrantsRepository.Verify(a => a.Update(It.Is<Entrant>(a => a.Payment != null), CancellationToken.None));
        }
    }
}
