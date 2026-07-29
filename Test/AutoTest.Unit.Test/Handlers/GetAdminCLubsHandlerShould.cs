using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AwesomeAssertions;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class GetAdminClubsHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IClubsRepository> _clubsRepository;
    private readonly IRequestHandler<GetAdminClubs, IEnumerable<ulong>> _sut;

    public GetAdminClubsHandlerShould()
    {
        _clubsRepository = _mr.Create<IClubsRepository>();
        _sut = new GetAdminClubsHandler(_clubsRepository.Object);
    }

    [Fact]
    public async Task ShouldReturnMatchingClubIds()
    {
        _clubsRepository
            .Setup(a => a.GetClubIdsByEmail("a@a.com", TestContext.Current.CancellationToken))
            .ReturnsAsync(new[] { 1ul, 2ul })
            .Verifiable(Times.Once);
        var res = await _sut.Handle(new("a@a.com"), TestContext.Current.CancellationToken);

        res.Should().BeEquivalentTo(new[] { 1ul, 2ul });
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ShouldReturnEmptyWhenNoMatch()
    {
        _clubsRepository
            .Setup(a => a.GetClubIdsByEmail("a@a.com", TestContext.Current.CancellationToken))
            .ReturnsAsync(Enumerable.Empty<ulong>())
            .Verifiable(Times.Once);
        var res = await _sut.Handle(new("a@a.com"), TestContext.Current.CancellationToken);

        res.Should().BeEquivalentTo(Enumerable.Empty<ulong>());
        _mr.VerifyAll();
    }
}
