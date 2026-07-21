using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence.Repositories;
using AutoTest.Unit.Test.Fixtures;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class ProfileRepositoryShould
{
    [Fact]
    public async Task GetReturnsProfile()
    {
        using var db = InMemDbFixture.GetDbContext();
        IProfileRepository sut = new ProfileRepository(db);

        var profile = new Profile("test@test.com", "Joe", "Bloggs", Age.Senior, false);
        db.Users.Add(profile);
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await sut.Get("test@test.com", TestContext.Current.CancellationToken);

        result.Should().NotBeNull();
        result!.GivenName.Should().Be("Joe");
    }

    [Fact]
    public async Task GetReturnsNullWhenNotFound()
    {
        using var db = InMemDbFixture.GetDbContext();
        IProfileRepository sut = new ProfileRepository(db);

        var result = await sut.Get("nobody@test.com", TestContext.Current.CancellationToken);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpsertAddsNewProfile()
    {
        using var db = InMemDbFixture.GetDbContext();
        IProfileRepository sut = new ProfileRepository(db);

        var profile = new Profile("new@test.com", "Jane", "Doe", Age.Junior, true);
        await sut.Upsert(profile, TestContext.Current.CancellationToken);

        var result = await db.Users.FindAsync(new object[] { "new@test.com" }, TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        result!.GivenName.Should().Be("Jane");
    }

    [Fact]
    public async Task UpsertUpdatesExistingProfile()
    {
        using var db = InMemDbFixture.GetDbContext();
        IProfileRepository sut = new ProfileRepository(db);

        var profile = new Profile("test@test.com", "Joe", "Bloggs", Age.Senior, false);
        await sut.Upsert(profile, TestContext.Current.CancellationToken);

        db.ChangeTracker.Clear();

        var updated = new Profile("test@test.com", "Joseph", "Bloggs", Age.Senior, false);
        await sut.Upsert(updated, TestContext.Current.CancellationToken);

        var result = await sut.Get("test@test.com", TestContext.Current.CancellationToken);
        result!.GivenName.Should().Be("Joseph");
    }
}
