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
    public class IsClubAdminShould
    {
        private readonly IRequestHandler<IsClubAdmin, bool> sut;
        private readonly Mock<IClubsRepository> clubsRepository;
        private readonly MockRepository mr;
        private readonly Mock<IEventsRepository> eventsRepository;

        public IsClubAdminShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            eventsRepository = mr.Create<IEventsRepository>();
            clubsRepository = mr.Create<IClubsRepository>();
            sut = new IsClubAdminHandler(clubsRepository.Object, eventsRepository.Object);
        }

        [Fact]
        public async Task ReturnFalse()
        {
            var eventId = 1ul;
            var clubId = 2ul;
            eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(
                new Event(eventId, clubId, "location", new DateTime(2000, 1, 1), 2, 3, "regs", EventType.AutoSolo, "maps", TimingSystem.App, new DateTime(), new DateTime(), 10)
                );
            var club = new Club(clubId, "club", "pay@paypal.com", "www.club.com");
            clubsRepository.Setup(a => a.GetById(clubId, CancellationToken.None)).ReturnsAsync(club);

            var res = await sut.Handle(new(eventId, "a@a.com"), CancellationToken.None);

            res.Should().BeFalse();
            mr.VerifyAll();
        }
    }
}
