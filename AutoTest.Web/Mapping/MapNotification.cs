using AutoTest.Domain.StorageModels;
using AutoTest.Web.Models.Save;

namespace AutoTest.Web.Mapping;

public static class MapNotification
{
    public static Notification Map(ulong notificationId, ulong eventId, string emailAddress, NotificationSaveModel notification)
    {
        return new Notification(notificationId, eventId, notification.Message, notification.Created, emailAddress);
    }
}
