using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence.Repositories;
using AutoTest.Unit.Test.Fixtures;
using FluentAssertions;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class EntrantsRepositoryShould
{
    [Fact]
    public async Task ReturnMatchingEmailIds()
    {
        using var db = InMemDbFixture.GetDbContext();
        IEntrantsRepository sut = new EntrantsRepository(db);

        var eventId = 10ul;
        db.Entrants.AddRange(
            new Entrant(1, 1, "Joe", "Bloggs", "j@test.com", "A", eventId, Age.Senior, false, null),
            new Entrant(2, 2, "Jane", "Doe", "j@test.com", "B", eventId, Age.Senior, false, null),
            new Entrant(3, 3, "Bob", "Smith", "b@test.com", "A", eventId, Age.Senior, false, null));
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = (await sut.GetEntrantIdsByEmail("j@test.com", TestContext.Current.CancellationToken)).ToArray();

        result.Should().BeEquivalentTo(new[] { 1ul, 2ul });
    }

    [Fact]
    public async Task ReturnEmptyWhenNoMatch()
    {
        using var db = InMemDbFixture.GetDbContext();
        IEntrantsRepository sut = new EntrantsRepository(db);

        db.Entrants.Add(new Entrant(1, 1, "Joe", "Bloggs", "j@test.com", "A", 10, Age.Senior, false, null));
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = (await sut.GetEntrantIdsByEmail("nobody@test.com", TestContext.Current.CancellationToken)).ToArray();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task MatchIsCaseSensitive()
    {
        using var db = InMemDbFixture.GetDbContext();
        IEntrantsRepository sut = new EntrantsRepository(db);

        db.Entrants.Add(new Entrant(1, 1, "Joe", "Bloggs", "Joe@Test.com", "A", 10, Age.Senior, false, null));
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = (await sut.GetEntrantIdsByEmail("joe@test.com", TestContext.Current.CancellationToken)).ToArray();

        result.Should().BeEmpty();
    }
}
