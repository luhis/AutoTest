using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup;

public class MarshalConfig : IEntityTypeConfiguration<Marshal>
{
    public void Configure(EntityTypeBuilder<Marshal> builder)
    {
        builder.HasKey(e => e.MarshalId);
        builder.Property(e => e.MarshalId).ValueGeneratedNever().IsRequired();
        builder.Property(e => e.RegistrationNumber).IsRequired();
        builder.Property(e => e.Role).IsRequired();
        builder.Property(e => e.GivenName).IsRequired();
        builder.Property(e => e.FamilyName).IsRequired();
        builder.Property(e => e.Email).IsRequired();
        builder.HasOne<Event>().WithMany().HasForeignKey(p => p.EventId);
        builder.OwnsOne(a => a.EmergencyContact, SetupEmergencyContact.Setup);
        builder.OwnsOne(a => a.AcceptDeclaration, SetupAcceptDeclaration.Setup);
        ///builder.HasIndex(a => new { a.FamilyName, a.GivenName });
    }
}
