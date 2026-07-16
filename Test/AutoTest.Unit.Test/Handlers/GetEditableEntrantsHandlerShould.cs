using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Persistence.Repositories;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.Fixtures;
using FluentAssertions;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class GetEditableEntrantsHandlerShould
{
    private readonly MockRepository mr;
    private readonly IRequestHandler<GetEditableEntrants, IEnumerable<ulong>> sut;
    private readonly AutoTestContext db;

    public GetEditableEntrantsHandlerShould()
    {
        mr = new MockRepository(MockBehavior.Strict);
        db = InMemDbFixture.GetDbContext();
        sut = new GetEditableEntrantsHandler(new EntrantsRepository(db));
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
        db.Entrants.AddRange(marshals);
        await db.SaveChangesAsync(cancellationToken);

        var res = await sut.Handle(new("test@test.com"), cancellationToken);

        res.Should().BeEquivalentTo(new[] { marshals.First().EntrantId });
        mr.VerifyAll();
    }
}
