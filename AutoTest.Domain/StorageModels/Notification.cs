using System;

namespace AutoTest.Domain.StorageModels;

public class Notification(ulong notificationId, ulong eventId, string message, DateTime created, string createdBy)
{
    public Notification() : this(0, 0, string.Empty, default, string.Empty) { }
    public ulong NotificationId { get; set; } = notificationId;

    public string Message { get; } = message;

    public DateTime Created { get; } = created;

    public string CreatedBy { get; } = createdBy;

    public ulong EventId { get; } = eventId;
}
