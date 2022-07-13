
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using FluentAssertions.ArgumentMatchers.Moq;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class SaveClubHandlerShould
    {
        private readonly IRequestHandler<SaveClub, ulong> sut;
        private readonly MockRepository mr;
        private readonly Mock<IClubsRepository> clubsRepository;
        private readonly Mock<ISignalRNotifier> signalRNotifier;

        public SaveClubHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            clubsRepository = mr.Create<IClubsRepository>();
            signalRNotifier = mr.Create<ISignalRNotifier>();
            sut = new SaveClubHandler(clubsRepository.Object, signalRNotifier.Object);
        }

        [Fact]
        public async Task InvokeSingnalRMessage()
        {
            var clubId = 1ul;
            var club = new Club(clubId, "My Club", "pay@pal.com", "clubsite.com");
            club.AdminEmails.Add(new AuthorisationEmail("test@test.com"));

            var clubFromDb = new Club(clubId, "My Club", "pay@pal.com", "clubsite.com");
            clubsRepository.Setup(a => a.GetById(clubId, CancellationToken.None)).ReturnsAsync(clubFromDb);
            clubsRepository.Setup(a => a.Upsert(club, CancellationToken.None)).Returns(Task.CompletedTask);
            signalRNotifier.Setup(a => a.NewClubAdmin(clubId, Its.EquivalentTo<IEnumerable<string>>(new[] { "test@test.com" }))).Returns(Task.CompletedTask);
            var se = new SaveClub(club);

            var res = await sut.Handle(se, CancellationToken.None);

            mr.VerifyAll();
        }

        [Fact]
        public async Task NotInvokeSingnalRMessage()
        {
            var clubId = 1ul;
            var club = new Club(clubId, "My Club", "pay|@pal.com", "clubsite.com");
            club.AdminEmails.Add(new AuthorisationEmail("test@test.com"));

            var clubFromDb = new Club(clubId, "My Club", "pay|@pal.com", "clubsite.com");
            clubFromDb.AdminEmails.Add(new AuthorisationEmail("Test@test.com"));
            clubsRepository.Setup(a => a.GetById(clubId, CancellationToken.None)).ReturnsAsync(clubFromDb);
            clubsRepository.Setup(a => a.Upsert(club, CancellationToken.None)).Returns(Task.CompletedTask);
            var se = new SaveClub(club);

            var res = await sut.Handle(se, CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
