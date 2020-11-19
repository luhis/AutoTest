﻿namespace AutoTest.Persistence.Setup
{
    using AutoTest.Domain.StorageModels;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SetupEvent
    {
        public static void Setup(EntityTypeBuilder<Event> entity)
        {
            entity.HasKey(e => e.EventId);
            entity.Property(e => e.EventId).ValueGeneratedNever().IsRequired();
            entity.Property(e => e.Location).IsRequired();
            entity.Property(e => e.StartTime).IsRequired();
            entity.Property(e => e.TestCount).IsRequired();
            entity.Property(e => e.MaxAttemptsPerTest).IsRequired();
            entity.Property(e => e.Regulations).IsRequired();
            entity.HasOne<Club>().WithMany().HasForeignKey(p => p.ClubId);
            entity.OwnsMany(a => a.MarshalEmails, SetupAuthorisationEmails.Setup);
            entity.OwnsMany(a => a.Tests, SetupTest.Setup);
        }
    }
}
