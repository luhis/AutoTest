using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup
{
    public static class SetupUser
    {
        public static void Setup(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).ValueGeneratedNever().IsRequired();
            entity.Property(e => e.GivenName).IsRequired();
            entity.Property(e => e.FamilyName).IsRequired();
            entity.Property(e => e.MsaLicense).IsRequired();
            entity.OwnsOne(a => a.Vehicle, SetupVehicle.Setup);
        }
    }
}
