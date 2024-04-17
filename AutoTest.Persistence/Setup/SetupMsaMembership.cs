using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup
{
    public static class SetupMsaMembership
    {
        public static void Setup<T>(OwnedNavigationBuilder<T, MsaMembership> entity) where T : class
        {
            entity.WithOwner();
            entity.Property(e => e.MsaLicenseType).IsRequired();
            entity.Property(e => e.MsaLicense).IsRequired();
        }
    }
}
