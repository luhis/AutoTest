using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.Fixtures;
using FluentAssertions;
using MediatR;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class DeleteEntrantShould
    {
        private readonly IRequestHandler<DeleteEntrant> sut;
        private readonly AutoTestContext context;

        public DeleteEntrantShould()
        {
            context = InMemDbFixture.GetDbContext();
            sut = new DeleteEntrantHandler(context);
        }

        [Fact(Skip = "Entity tracking issue")]
        public async Task ReturnBlankProfileIfNone()
        {
            context.Entrants!.Should().BeEmpty();

            var eventId = 1ul;
            var entrantId = 2ul;
            context.Entrants!.Add(new(entrantId, 22, "joe", "bloggs", "joe@bloggs.com", "A", eventId, "BRMC", 1234, Domain.Enums.Age.Senior));
            await context.SaveChangesAsync();

            await sut.Handle(new(eventId, entrantId), CancellationToken.None);

            context.Users!.Should().BeEmpty();
        }
    }
}
