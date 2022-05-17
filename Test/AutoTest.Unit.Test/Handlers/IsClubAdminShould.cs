using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.Fixtures;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class IsClubAdminShould
    {
        private readonly IRequestHandler<IsClubAdmin, bool> sut;
        private readonly AutoTestContext context;
        private readonly MockRepository mr;
        private readonly Mock<IEventsRepository> eventsRepository;

        public IsClubAdminShould()
        {
            context = InMemDbFixture.GetDbContext();
            mr = new MockRepository(MockBehavior.Strict);
            eventsRepository = mr.Create<IEventsRepository>();
            sut = new IsClubAdminHandler(context, eventsRepository.Object);
        }

        [Fact]
        public async Task ReturnFalse()
        {
            var eventId = 1ul;
            var clubId = 2ul;
            eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(
                new Event(eventId, clubId, "location", new System.DateTime(2000, 1, 1), 2, 3, "regs", Domain.Enums.EventType.AutoSolo, "maps", Domain.Enums.TimingSystem.App)
                );
            context.Clubs!.Add(new(clubId, "club", "pay@paypal.com", "www.club.com"));
            await context.SaveChangesAsync();

            var res = await sut.Handle(new(eventId, "a@a.com"), CancellationToken.None);

            res.Should().BeFalse();
            mr.VerifyAll();
        }
    }
}
