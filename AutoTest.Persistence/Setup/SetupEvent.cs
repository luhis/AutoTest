namespace AutoTest.Persistence.Setup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoTest.Domain.Enums;
    using AutoTest.Domain.StorageModels;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SetupEvent
    {
        public static void Setup(EntityTypeBuilder<Event> entity)
        {
            entity.HasKey(e => e.EventId);
            entity.Property(e => e.EventId).ValueGeneratedNever().IsRequired();
            entity.Property(e => e.Location).IsRequired();
            entity.Property(e => e.StartTime).IsRequired();
            entity.Property(e => e.EntryOpenDate).IsRequired();
            entity.Property(e => e.EntryCloseDate).IsRequired();
            entity.Property(e => e.TestCount).IsRequired();
            entity.Property(e => e.MaxAttemptsPerTest).IsRequired();
            entity.Property(e => e.Regulations).IsRequired();
            entity.Property(e => e.Maps).IsRequired();
            entity.Property(e => e.EventTypes).IsRequired().HasConversion(
                a => string.Join(",", a.Select(t => (int)t)),
                a => a!.Split(",", System.StringSplitOptions.None).Select(e => int.Parse(e)).Cast<EventType>().ToArray());
            var valueComparer = new ValueComparer<ICollection<EventType>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());
            entity.Property(e => e.EventTypes).Metadata.SetValueComparer(valueComparer);
            entity.Property(e => e.EventStatus).IsRequired();
            entity.Property(e => e.TimingSystem).IsRequired();
            entity.Property(e => e.MaxEntrants).IsRequired();
            entity.HasOne<Club>().WithMany().HasForeignKey(p => p.ClubId);
            entity.OwnsMany(a => a.Tests, SetupTest.Setup);
        }
    }
}
