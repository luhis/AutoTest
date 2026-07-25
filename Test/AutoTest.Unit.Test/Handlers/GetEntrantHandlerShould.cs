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

public class GetEntrantHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IEntrantsRepository> _entrantsRepository;
    private readonly IRequestHandler<GetEntrant, Entrant?> _sut;

    public GetEntrantHandlerShould()
    {
        _entrantsRepository = _mr.Create<IEntrantsRepository>();
        _sut = new GetEntrantHandler(_entrantsRepository.Object);
    }

    [Fact]
    public async Task GetEntrant()
    {
        var eventId = 1ul;
        var entrantId = (ushort)2u;
        var entrant = new Entrant(1, entrantId, "Joe", "Bloggs", "a@a.com", "A", 99, Domain.Enums.Age.Senior, false, null);
        _entrantsRepository.Setup(a => a.GetById(eventId, entrantId, TestContext.Current.CancellationToken)).ReturnsAsync(entrant);

        var res = await _sut.Handle(new(eventId, entrantId), TestContext.Current.CancellationToken);

        res.Should().BeEquivalentTo(entrant);
        _mr.VerifyAll();
    }
}
