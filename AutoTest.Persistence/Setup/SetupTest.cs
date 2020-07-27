namespace AutoTest.Persistence.Setup
{
    using AutoTest.Domain.StorageModels;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SetupTest
    {
        public static void Setup<T>(OwnedNavigationBuilder<T, Test> entity) where T : class
        {
            entity.Property(e => e.Ordinal).IsRequired();
            entity.Property(e => e.MapLocation).IsRequired();
        }
    }
}
