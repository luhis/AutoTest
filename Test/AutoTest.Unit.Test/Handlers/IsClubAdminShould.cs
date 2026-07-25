using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
using AwesomeAssertions;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class IsClubAdminShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IClubsRepository> _clubsRepository;
    private readonly Mock<IEventsRepository> _eventsRepository;
    private readonly IRequestHandler<IsClubAdmin, bool> _sut;

    public IsClubAdminShould()
    {
        _eventsRepository = _mr.Create<IEventsRepository>();
        _clubsRepository = _mr.Create<IClubsRepository>();
        _sut = new IsClubAdminHandler(_clubsRepository.Object, _eventsRepository.Object);
    }

    [Fact]
    public async Task ReturnFalse()
    {
        var eventId = 1ul;
        var clubId = 2ul;
        _eventsRepository.Setup(a => a.GetById(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(
            Models.GetEvent(eventId, clubId)
            );
        var club = new Club(clubId, "club", "pay@paypal.com", "www.club.com");
        _clubsRepository.Setup(a => a.GetById(clubId, TestContext.Current.CancellationToken)).ReturnsAsync(club);

        var res = await _sut.Handle(new(eventId, "a@a.com"), TestContext.Current.CancellationToken);

        res.Should().BeFalse();
        _mr.VerifyAll();
    }
}
