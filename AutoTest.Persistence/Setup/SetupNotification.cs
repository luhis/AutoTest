using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup
{
    public static class SetupNotification
    {
        public static void Setup(EntityTypeBuilder<Notification> entity)
        {
            entity.HasKey(e => e.NotificationId);
            entity.Property(e => e.Message).IsRequired();
            entity.Property(e => e.Created).IsRequired();
            entity.Property(e => e.CreatedBy).IsRequired();
            entity.HasOne<Event>().WithMany().HasForeignKey(p => p.EventId);
        }
    }
}
