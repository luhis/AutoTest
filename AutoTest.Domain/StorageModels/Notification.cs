using System;

namespace AutoTest.Domain.StorageModels
{
    public class Notification
    {
        public Notification(ulong notificationId, ulong eventId, string message, DateTime created, string createdBy)
        {
            Message = message;
            Created = created;
            CreatedBy = createdBy;
            EventId = eventId;
            NotificationId = notificationId;
        }

        public ulong NotificationId { get; }

        public string Message { get; }
        public DateTime Created { get; }

        public string CreatedBy { get; }

        public ulong EventId { get; }
    }
}
