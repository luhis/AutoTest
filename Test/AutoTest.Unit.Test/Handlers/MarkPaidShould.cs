using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.Fixtures;
using FluentAssertions;
using MediatR;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class MarkPaidShould
    {
        private readonly IRequestHandler<MarkPaid, MediatR.Unit> sut;
        private readonly AutoTestContext context;

        public MarkPaidShould()
        {
            context = InMemDbFixture.GetDbContext();
            sut = new MarkPaidHandler(context);
        }

        [Fact]
        public async Task MarkNotPaid()
        {
            var entrantId = 1ul;
            var eventId = 22ul;
            var entrant = new Entrant(entrantId, 1, "matt", "mccorry", "a@a.com", "A", eventId, "BRMC", 1234, Domain.Enums.Age.Senior);
            entrant.SetPayment(new(new System.DateTime(2000, 1, 1), Domain.Enums.PaymentMethod.Paypal, new System.DateTime(2000, 2, 2)));
            context.Entrants!.Add(entrant);
            await context.SaveChangesAsync();
            context.Entry(entrant).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            await sut.Handle(new(eventId, entrantId, null), CancellationToken.None);

            //var updated = context.Entrants!.Single(a => a.EntrantId == entrantId);
            //context.Entry(entrant).Reload();
            //entrant.Payment.Should().BeNull();
        }

        [Fact]
        public async Task MarkPaid()
        {
            var entrantId = 1ul;
            var eventId = 22ul;
            var entrant = new Entrant(entrantId, 1, "matt", "mccorry", "a@a.com", "A", eventId, "BRMC", 1234, Domain.Enums.Age.Senior);
            context.Entrants!.Add(entrant);
            await context.SaveChangesAsync();
            context.Entry(entrant).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            await sut.Handle(new(eventId, entrantId, new(new System.DateTime(2000, 1, 1), Domain.Enums.PaymentMethod.Paypal, new System.DateTime(2000, 2, 2))), CancellationToken.None);

            //var updated = context.Entrants!.Single(a => a.EntrantId == entrantId);
            //updated.Payment.Should().NotBeNull();
            //updated.Payment!.Method.Should().Be(Domain.Enums.PaymentMethod.Paypal);
        }
    }
}
