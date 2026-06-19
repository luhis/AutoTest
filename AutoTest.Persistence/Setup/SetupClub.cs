using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup;

public class ClubConfig : IEntityTypeConfiguration<Club>
{
    public void Configure(EntityTypeBuilder<Club> builder)
    {
        builder.HasKey(e => e.ClubId);
        builder.Property(e => e.ClubId).ValueGeneratedNever().IsRequired();
        builder.Property(e => e.ClubName).IsRequired();
        builder.Property(e => e.ClubPaymentAddress).IsRequired();
        builder.Property(e => e.Website).IsRequired();
        builder.OwnsMany(a => a.AdminEmails, SetupAuthorisationEmails.Setup);
    }
}
