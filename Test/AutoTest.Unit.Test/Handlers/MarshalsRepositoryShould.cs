using System.Linq;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Persistence.Repositories;
using AutoTest.Unit.Test.Fixtures;
using AutoTest.Unit.Test.MockData;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class MarshalsRepositoryShould
{
    [Fact]
    public async Task GetByIdReturnsMarshal()
    {
        using var db = InMemDbFixture.GetDbContext();
        IMarshalsRepository sut = new MarshalsRepository(db);

        var marshal = Models.GetMarshal(1, 10, "marshal@test.com");
        db.Marshals.Add(marshal);
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await sut.GetById(10, 1, TestContext.Current.CancellationToken);

        result.Should().NotBeNull();
        result!.Email.Should().Be("marshal@test.com");
    }

    [Fact]
    public async Task GetByIdReturnsNullWhenNotFound()
    {
        using var db = InMemDbFixture.GetDbContext();
        IMarshalsRepository sut = new MarshalsRepository(db);

        var result = await sut.GetById(999, 999, TestContext.Current.CancellationToken);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByEmailReturnsMarshals()
    {
        using var db = InMemDbFixture.GetDbContext();
        IMarshalsRepository sut = new MarshalsRepository(db);

        db.Marshals.AddRange(
            Models.GetMarshal(1, 10, "j@test.com"),
            Models.GetMarshal(2, 10, "j@test.com"),
            Models.GetMarshal(3, 10, "other@test.com"));
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = sut.GetByEmail("j@test.com").ToArray();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByEventIdReturnsMarshals()
    {
        using var db = InMemDbFixture.GetDbContext();
        IMarshalsRepository sut = new MarshalsRepository(db);

        db.Marshals.AddRange(
            Models.GetMarshal(1, 10, "a@test.com"),
            Models.GetMarshal(2, 10, "b@test.com"),
            Models.GetMarshal(3, 20, "c@test.com"));
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = sut.GetByEventId(10).ToArray();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpsertAddsNewMarshal()
    {
        using var db = InMemDbFixture.GetDbContext();
        IMarshalsRepository sut = new MarshalsRepository(db);

        var marshal = Models.GetMarshal(1, 10, "new@test.com");
        await sut.Upsert(marshal, TestContext.Current.CancellationToken);

        var result = await db.Marshals.FindAsync(new object[] { 1UL }, TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        result!.Email.Should().Be("new@test.com");
    }

    [Fact]
    public async Task RemoveDeletesMarshal()
    {
        using var db = InMemDbFixture.GetDbContext();
        IMarshalsRepository sut = new MarshalsRepository(db);

        var marshal = Models.GetMarshal(1, 10, "remove@test.com");
        await sut.Upsert(marshal, TestContext.Current.CancellationToken);

        await sut.Remove(marshal, TestContext.Current.CancellationToken);

        var result = await sut.GetById(10, 1, TestContext.Current.CancellationToken);
        result.Should().BeNull();
    }
}
