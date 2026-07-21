using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Persistence.Repositories;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.Fixtures;
using AwesomeAssertions;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class GetEditableEntrantsHandlerShould
{
    private readonly MockRepository _mr;
    private readonly IRequestHandler<GetEditableEntrants, IEnumerable<ulong>> _sut;
    private readonly AutoTestContext _db;

    public GetEditableEntrantsHandlerShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
        _db = InMemDbFixture.GetDbContext();
        _sut = new GetEditableEntrantsHandler(new EntrantsRepository(_db));
    }

    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD103:Call async methods when in an async method", Justification = "In-memory database")]
    public async Task GetEntrants()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var marshals = new[] {
            new Entrant(1, 22, "Joe", "Bloggs", "test@test.com", "A", 99, Domain.Enums.Age.Senior, false, null),
            new Entrant(2, 22, "Joe", "Bloggs", "a@a.com", "A", 99, Domain.Enums.Age.Senior, false, null)
        };
        _db.Entrants.AddRange(marshals);
        await _db.SaveChangesAsync(cancellationToken);

        var res = await _sut.Handle(new("test@test.com"), cancellationToken);

        res.Should().BeEquivalentTo(new[] { marshals.First().EntrantId });
        _mr.VerifyAll();
    }
}
