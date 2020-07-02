using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup
{
    public static class SetupVehicle
    {
        public static void Setup<T>(OwnedNavigationBuilder<T, Vehicle> entity) where T : class
        {
            entity.WithOwner();
            entity.Property(e => e.Displacement).IsRequired();
            entity.Property(e => e.Make).IsRequired();
            entity.Property(e => e.Model).IsRequired();
            entity.Property(e => e.Registration).IsRequired();
            entity.Property(e => e.Year).IsRequired();
        }
    }
}
