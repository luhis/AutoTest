using System.Linq;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence.Repositories;
using AutoTest.Unit.Test.Fixtures;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class ClubRepositoryShould
{
    [Fact]
    public async Task GetByIdReturnsClub()
    {
        using var db = InMemDbFixture.GetDbContext();
        IClubsRepository sut = new ClubRepository(db);

        var club = new Club(1, "Test Club", "pay@test.com", "https://test.com");
        club.SetAdminEmails(new[] { new AuthorisationEmail("admin@test.com") });
        db.Clubs.Add(club);
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await sut.GetById(1, TestContext.Current.CancellationToken);

        result.Should().NotBeNull();
        result!.ClubName.Should().Be("Test Club");
    }

    [Fact]
    public async Task GetByIdReturnsNullWhenNotFound()
    {
        using var db = InMemDbFixture.GetDbContext();
        IClubsRepository sut = new ClubRepository(db);

        var result = await sut.GetById(999, TestContext.Current.CancellationToken);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllReturnsAllClubs()
    {
        using var db = InMemDbFixture.GetDbContext();
        IClubsRepository sut = new ClubRepository(db);

        db.Clubs.AddRange(
            new Club(1, "Alpha Club", "a@test.com", ""),
            new Club(2, "Beta Club", "b@test.com", ""));
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = (await sut.GetAll(TestContext.Current.CancellationToken)).ToArray();

        result.Should().BeEquivalentTo(new[]
        {
            new { ClubName = "Alpha Club" },
            new { ClubName = "Beta Club" },
        }, o => o.WithStrictOrdering());
    }

    [Fact]
    public async Task UpsertAddsNewClub()
    {
        using var db = InMemDbFixture.GetDbContext();
        IClubsRepository sut = new ClubRepository(db);

        var club = new Club(1, "New Club", "pay@test.com", "");
        await sut.Upsert(club, TestContext.Current.CancellationToken);

        var result = await db.Clubs.FindAsync(new object[] { 1UL }, TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        result!.ClubName.Should().Be("New Club");
    }

    [Fact]
    public async Task DeleteRemovesClub()
    {
        using var db = InMemDbFixture.GetDbContext();
        IClubsRepository sut = new ClubRepository(db);

        var club = new Club(1, "To Delete", "pay@test.com", "");
        await sut.Upsert(club, TestContext.Current.CancellationToken);

        db.ChangeTracker.Clear();

        await sut.Delete(1, TestContext.Current.CancellationToken);

        var result = await sut.GetById(1, TestContext.Current.CancellationToken);
        result.Should().BeNull();
    }
}
