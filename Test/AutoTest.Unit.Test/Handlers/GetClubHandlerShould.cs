using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AwesomeAssertions;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class GetClubHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IClubsRepository> _clubsRepository;
    private readonly IRequestHandler<GetClub, Club?> _sut;

    public GetClubHandlerShould()
    {
        _clubsRepository = _mr.Create<IClubsRepository>();
        _sut = new GetClubHandler(_clubsRepository.Object);
    }

    [Fact]
    public async Task ReturnNullIfNotClub()
    {
        var clubId = 1ul;
        _clubsRepository.Setup(a => a.GetById(clubId, TestContext.Current.CancellationToken)).ReturnsAsync((Club?)null);

        var res = await _sut.Handle(new(clubId), TestContext.Current.CancellationToken);

        res.Should().BeNull();
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ReturnClub()
    {
        var clubId = 1ul;
        var club = new Club(clubId, "First", "Last", "");
        _clubsRepository.Setup(a => a.GetById(clubId, TestContext.Current.CancellationToken)).ReturnsAsync(club);

        var res = await _sut.Handle(new(clubId), TestContext.Current.CancellationToken);

        res.Should().BeEquivalentTo(club);
        _mr.VerifyAll();
    }
}
