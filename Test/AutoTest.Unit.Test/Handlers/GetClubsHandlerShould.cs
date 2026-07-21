using System.Collections.Generic;
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

public class GetClubsHandlerShould
{
    private readonly MockRepository _mr;
    private readonly IRequestHandler<GetClubs, IEnumerable<Club>> _sut;
    private readonly Mock<IClubsRepository> _clubsRepository;

    public GetClubsHandlerShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
        _clubsRepository = _mr.Create<IClubsRepository>();
        _sut = new GetClubsHandler(_clubsRepository.Object);
    }

    [Fact]
    public async Task ReturnExistingClubs()
    {
        var clubId = 1ul;
        var clubs = new[] { new Club(clubId, "First", "Last", "") };
        _clubsRepository.Setup(a => a.GetAll(TestContext.Current.CancellationToken)).ReturnsAsync(clubs);

        var res = await _sut.Handle(new(), TestContext.Current.CancellationToken);

        res.Should().BeEquivalentTo(clubs);
        _mr.VerifyAll();
    }
}
