using System;

namespace AutoTest.Domain.StorageModels;

public record Notification(ulong NotificationId, ulong EventId, string Message, DateTime Created, string CreatedBy);
