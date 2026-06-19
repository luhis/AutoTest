using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup;

public class EntrantConfig : IEntityTypeConfiguration<Entrant>
{
    public void Configure(EntityTypeBuilder<Entrant> builder)
    {
        builder.HasDiscriminatorInJsonId();
        builder.HasKey(e => e.EntrantId);
        builder.Property(e => e.EntrantId).ValueGeneratedNever().IsRequired();
        builder.Property(e => e.DriverNumber).IsRequired();
        builder.Property(e => e.Class).IsRequired();
        builder.Property(e => e.GivenName).IsRequired();
        builder.Property(e => e.FamilyName).IsRequired();
        builder.Property(e => e.Email).IsRequired();
        builder.Property(e => e.Age).IsRequired();
        builder.Property(e => e.EntrantStatus).IsRequired();
        builder.Property(a => a.IsLady).IsRequired();
        builder.HasOne<Event>().WithMany().HasForeignKey(p => p.EventId);
        builder.OwnsOne(a => a.Vehicle, SetupVehicle.Setup);
        builder.OwnsOne(a => a.EmergencyContact, SetupEmergencyContact.Setup);
        builder.OwnsOne(a => a.MsaMembership, SetupMsaMembership.Setup);
        builder.OwnsOne(a => a.AcceptDeclaration, SetupAcceptDeclaration.Setup);
        builder.OwnsOne(a => a.Payment, SetupPayment.Setup);
        builder.OwnsOne(a => a.EntrantClub, SetupEntrantClub.Setup);
        builder.HasOne<Entrant>().WithMany().HasForeignKey(p => p.DoubleDrivenWith);
    }
}
