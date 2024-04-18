using System;

namespace AutoTest.Domain.StorageModels
{
    public class Notification(ulong notificationId, ulong eventId, string message, DateTime created, string createdBy)
    {
        public ulong NotificationId { get; } = notificationId;

        public string Message { get; } = message;

        public DateTime Created { get; } = created;

        public string CreatedBy { get; } = createdBy;

        public ulong EventId { get; } = eventId;
    }
}
