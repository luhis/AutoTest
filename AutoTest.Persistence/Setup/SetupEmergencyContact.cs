using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup
{
    public static class SetupEmergencyContact
    {
        public static void Setup<T>(OwnedNavigationBuilder<T, EmergencyContact> entity) where T : class
        {
            entity.WithOwner();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Phone).IsRequired();
        }
    }
}
