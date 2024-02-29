using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Persistence.Repositories
{
    public class NotificationRepository(AutoTestContext autoTestContext) : INotificationsRepository
    {
        async Task INotificationsRepository.AddNotification(Notification notification, CancellationToken cancellationToken)
        {
            await autoTestContext.Notifications!.Upsert(notification, a => a.NotificationId == notification.NotificationId, cancellationToken);
            await autoTestContext.SaveChangesAsync(cancellationToken);
        }

        Task<IEnumerable<Notification>> INotificationsRepository.GetNotifications(ulong eventId, CancellationToken cancellationToken)
        {
            return autoTestContext.Notifications!.Where(a => a.EventId == eventId).OrderByDescending(a => a.Created).ToEnumerableAsync(cancellationToken);
        }
    }
}
