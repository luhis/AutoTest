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
    public class IsClubAdminShould
    {
        private readonly IRequestHandler<IsClubAdmin, bool> sut;
        private readonly AutoTestContext context;

        public IsClubAdminShould()
        {
            context = InMemDbFixture.GetDbContext();
            sut = new IsClubAdminHandler(context);
        }

        [Fact]
        public async Task ReturnFalse()
        {
            var eventId = 1ul;
            var clubId = 2ul;
            context.Events!.Add(new(eventId, clubId, "location", new System.DateTime(2000, 1, 1), 2, 3, "regs", Domain.Enums.EventType.AutoSolo, "maps", Domain.Enums.TimingSystem.App));
            context.Clubs!.Add(new(clubId, "club", "pay@paypal.com", "www.club.com"));
            context.SaveChanges();
            var res = await sut.Handle(new(eventId, "a@a.com"), CancellationToken.None);

            res.Should().BeFalse();
        }
    }
}
