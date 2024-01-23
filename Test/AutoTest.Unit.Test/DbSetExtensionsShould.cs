using AutoTest.Unit.Test.Fixtures;
using Xunit;
using AutoTest.Persistence;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using FluentAssertions;
using static AutoTest.Persistence.DbSetExtensions;

namespace AutoTest.Unit.Test
{
    public class DbSetExtensionsShould
    {
        [Fact]
        public async Task Insert()
        {
            var db = InMemDbFixture.GetDbContext();
            var method = await db.Marshals!.Upsert(new Domain.StorageModels.Marshal(1, "", "", "", 2, 3, ""), a => a.MarshalId == 1, CancellationToken.None);

            await db.SaveChangesAsync();

            db.Marshals!.Count().Should().Be(1);
            method.Should().Be(UpdateStatus.Add);
        }

        //[Fact]
        //public async Task Update()
        //{

        //    var db = InMemDbFixture.GetDbContext();
        //    db.Marshals!.Add(new Domain.StorageModels.Marshal(1, "", "", "", 2, 3, ""));
        //    await db.SaveChangesAsync();
        //    db.ChangeTracker.Clear();
        //    var method = await db.Marshals!.Upsert(new Domain.StorageModels.Marshal(1, "", "", "", 2, 3, ""), a => a.MarshalId == 1, CancellationToken.None);

        //    await db.SaveChangesAsync();

        //    db.Marshals!.Count().Should().Be(1);
        //    method.Should().Be(UpdateStatus.Update);
        //}
    }
}
