using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup;

public class NotificationConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasDiscriminator<string>(nameof(Notification));
        builder.HasKey(e => e.NotificationId);
        builder.Property(e => e.Message).IsRequired();
        builder.Property(e => e.Created).IsRequired();
        builder.Property(e => e.CreatedBy).IsRequired();
        builder.HasOne<Event>().WithMany().HasForeignKey(p => p.EventId);
    }
}
