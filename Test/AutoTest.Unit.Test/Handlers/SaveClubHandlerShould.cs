
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using AwesomeAssertions.ArgumentMatchers.Moq;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class SaveClubHandlerShould
{
    private readonly IRequestHandler<SaveClub, ulong> _sut;
    private readonly MockRepository _mr;
    private readonly Mock<IClubsRepository> _clubsRepository;
    private readonly Mock<IAuthorisationNotifier> _signalRNotifier;

    public SaveClubHandlerShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
        _clubsRepository = _mr.Create<IClubsRepository>();
        _signalRNotifier = _mr.Create<IAuthorisationNotifier>();
        _sut = new SaveClubHandler(_clubsRepository.Object, _signalRNotifier.Object);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async Task InvokeSignalRMessage(bool newClubHasEmail, bool dbClubHasEmail)
    {
        var clubId = 1ul;
        var club = new Club(clubId, "My Club", "pay@pal.com", "clubsite.com");
        if (newClubHasEmail)
            club.AdminEmails.Add(new AuthorisationEmail("test@test.com"));

        var clubFromDb = new Club(clubId, "My Club", "pay@pal.com", "clubsite.com");
        if (dbClubHasEmail)
            clubFromDb.AdminEmails.Add(new AuthorisationEmail("test@test.com"));

        _clubsRepository.Setup(a => a.GetById(clubId, TestContext.Current.CancellationToken)).ReturnsAsync(clubFromDb);
        _clubsRepository.Setup(a => a.Upsert(club, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        if (newClubHasEmail && !dbClubHasEmail)
            _signalRNotifier.Setup(a => a.NewClubAdmin(clubId, Its.EquivalentTo<IEnumerable<string>>(new[] { "test@test.com" }), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);
        if (!newClubHasEmail && dbClubHasEmail)
            _signalRNotifier.Setup(a => a.RemoveClubAdmin(clubId, Its.EquivalentTo<IEnumerable<string>>(new[] { "test@test.com" }), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        var se = new SaveClub(club);
        var res = await _sut.Handle(se, TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }

    [Fact]
    public async Task NotInvokeSignalRMessage()
    {
        var clubId = 1ul;
        var club = new Club(clubId, "My Club", "pay|@pal.com", "clubsite.com");
        club.AdminEmails.Add(new AuthorisationEmail("test@test.com"));

        var clubFromDb = new Club(clubId, "My Club", "pay|@pal.com", "clubsite.com");
        clubFromDb.AdminEmails.Add(new AuthorisationEmail("Test@test.com"));
        _clubsRepository.Setup(a => a.GetById(clubId, TestContext.Current.CancellationToken)).ReturnsAsync(clubFromDb);
        _clubsRepository.Setup(a => a.Upsert(club, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);
        var se = new SaveClub(club);

        var res = await _sut.Handle(se, TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }
}
