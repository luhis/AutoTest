using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup;

public class ProfileConfig : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasDiscriminator<string>(nameof(Profile));
        builder.HasKey(e => e.EmailAddress);
        builder.Property(e => e.EmailAddress).ValueGeneratedNever().IsRequired();
        builder.Property(e => e.GivenName).IsRequired();
        builder.Property(e => e.FamilyName).IsRequired();
        builder.Property(e => e.Age).IsRequired();
        builder.Property(e => e.IsLady).IsRequired();
        builder.OwnsOne(a => a.Vehicle, SetupVehicle.Setup);
        builder.OwnsOne(a => a.EmergencyContact, SetupEmergencyContact.Setup);
        builder.OwnsMany(a => a.ClubMemberships, SetupClubMembership.Setup);
        builder.OwnsOne(a => a.MsaMembership, SetupMsaMembership.Setup);
    }
}
