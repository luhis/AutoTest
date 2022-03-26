using System;
using AutoTest.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Unit.Test.Fixtures
{
    public static class InMemDbFixture
    {
        public static AutoTestContext GetDbContext() => new AutoTestContext(new DbContextOptionsBuilder<AutoTestContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options);
    }
}
