﻿namespace AutoTest.Persistence.Setup
{
    using AutoTest.Domain.StorageModels;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SetupEntrant
    {
        public static void Setup(EntityTypeBuilder<Entrant> entity)
        {
            entity.HasKey(e => e.EntrantId);
            entity.Property(e => e.EntrantId).ValueGeneratedNever().IsRequired();
            entity.Property(e => e.DriverNumber).IsRequired();
            entity.Property(e => e.Class).IsRequired();
            entity.Property(e => e.GivenName).IsRequired();
            entity.Property(e => e.FamilyName).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Age).IsRequired();
            entity.Property(e => e.EntrantStatus).IsRequired();
            entity.Property(a => a.IsLady).IsRequired();
            entity.HasOne<Event>().WithMany().HasForeignKey(p => p.EventId);
            entity.OwnsOne(a => a.Vehicle, SetupVehicle.Setup);
            entity.OwnsOne(a => a.EmergencyContact, SetupEmergencyContact.Setup);
            entity.OwnsOne(a => a.MsaMembership, SetupMsaMembership.Setup);
            entity.OwnsOne(a => a.AcceptDeclaration, SetupAcceptDeclaration.Setup);
            entity.OwnsOne(a => a.Payment, SetupPayment.Setup);
            entity.OwnsOne(a => a.EntrantClub, SetupEntrantClub.Setup);
            entity.HasOne<Entrant>().WithMany().HasForeignKey(p => p.DoubleDrivenWith);
        }
    }
}
