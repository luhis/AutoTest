using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Unit.Test.Fixtures;
using AwesomeAssertions;
using Xunit;
using static AutoTest.Persistence.DbSetExtensions;

namespace AutoTest.Unit.Test;

public class DbSetExtensionsShould
{
    [Fact]
    public async Task Insert()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var db = InMemDbFixture.GetDbContext();
        var method = await db.Marshals.Upsert(new Domain.StorageModels.Marshal(1, "", "", "", 2, 3, ""), a => a.MarshalId == 1, cancellationToken);

        await db.SaveChangesAsync(cancellationToken);

        db.Marshals.Count().Should().Be(1);
        method.Should().Be(UpdateStatus.Add);
    }

    [Fact]
    public async Task Update()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var db = InMemDbFixture.GetDbContext();
        db.Marshals.Add(new Domain.StorageModels.Marshal(1, "", "", "", 2, 3, ""));
        await db.SaveChangesAsync(cancellationToken);
        db.ChangeTracker.Clear();
        var method = await db.Marshals.Upsert(new Domain.StorageModels.Marshal(1, "", "", "", 2, 3, ""), a => a.MarshalId == 1ul, cancellationToken);

        await db.SaveChangesAsync(cancellationToken);

        db.Marshals.Count().Should().Be(1);
        method.Should().Be(UpdateStatus.Update);
    }
}
