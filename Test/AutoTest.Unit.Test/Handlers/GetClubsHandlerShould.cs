using System.Collections.Generic;
using System.Threading;
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
    private readonly MockRepository mr;
    private readonly IRequestHandler<GetClubs, IEnumerable<Club>> sut;
    private readonly Mock<IClubsRepository> clubsRepository;

    public GetClubsHandlerShould()
    {
        mr = new MockRepository(MockBehavior.Strict);
        clubsRepository = mr.Create<IClubsRepository>();
        sut = new GetClubsHandler(clubsRepository.Object);
    }

    [Fact]
    public async Task ReturnExistingClubs()
    {
        var clubId = 1ul;
        var clubs = new[] { new Club(clubId, "First", "Last", "") };
        clubsRepository.Setup(a => a.GetAll(CancellationToken.None)).ReturnsAsync(clubs);

        var res = await sut.Handle(new(), CancellationToken.None);

        res.Should().BeEquivalentTo(clubs);
        mr.VerifyAll();
    }
}
