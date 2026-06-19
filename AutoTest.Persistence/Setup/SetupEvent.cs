using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup;

public class EventConfig : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasDiscriminator<string>(nameof(Event));
        builder.HasKey(e => e.EventId);
        builder.Property(e => e.EventId).ValueGeneratedNever().IsRequired();
        builder.Property(e => e.Location).IsRequired();
        builder.Property(e => e.StartTime).IsRequired();
        builder.Property(e => e.EntryOpenDate).IsRequired();
        builder.Property(e => e.EntryCloseDate).IsRequired();
        builder.Property(e => e.CourseCount).IsRequired();
        builder.Property(e => e.MaxAttemptsPerCourse).IsRequired();
        builder.Property(e => e.Regulations).IsRequired();
        builder.Property(e => e.Maps).IsRequired();
        builder.Property(e => e.EventTypes).IsRequired().HasConversion(
            a => string.Join(",", a.Select(t => (int)t)),
            a => a!.Split(",", StringSplitOptions.None).Where(a => !string.IsNullOrEmpty(a)).Select(e => (EventType)int.Parse(e, CultureInfo.InvariantCulture)).ToArray());
        var valueComparer = new ValueComparer<ICollection<EventType>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());
        builder.Property(e => e.EventTypes).Metadata.SetValueComparer(valueComparer);
        builder.Property(e => e.EventStatus).IsRequired();
        builder.Property(e => e.TimingSystem).IsRequired();
        builder.Property(e => e.MaxEntrants).IsRequired();
        builder.HasOne<Club>().WithMany().HasForeignKey(p => p.ClubId);
        builder.OwnsMany(a => a.Courses, SetupTest.Setup);
        builder.Property(e => e.Created).IsRequired();
    }
}
