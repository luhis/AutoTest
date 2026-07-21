using System.Collections.Generic;
using System.Linq;
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

public class GetEntrantsHandlerShould
{
    private readonly MockRepository _mr;
    private readonly IRequestHandler<GetEntrants, IEnumerable<Entrant>> _sut;
    private readonly Mock<IEntrantsRepository> _entrantsRepository;

    public GetEntrantsHandlerShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
        _entrantsRepository = _mr.Create<IEntrantsRepository>();
        _sut = new GetEntrantsHandler(_entrantsRepository.Object);
    }

    [Fact]
    public async Task GetEntrants()
    {
        var eventId = 1ul;
        var entrants = new[] {
            new Entrant(1, 22, "Joe", "Bloggs", "a@a.com", "A", 99, Domain.Enums.Age.Senior, false, null),
            new Entrant(2, 22, "Joe", "Bloggs", "a@a.com", "A", 99, Domain.Enums.Age.Senior, false, null)
        };
        _entrantsRepository.Setup(a => a.GetAll(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(entrants);

        var res = await _sut.Handle(new(eventId), TestContext.Current.CancellationToken);

        res.Should().BeEquivalentTo(entrants.OrderBy(a => a.FamilyName), o => o.WithStrictOrdering());
        _mr.VerifyAll();
    }
}
