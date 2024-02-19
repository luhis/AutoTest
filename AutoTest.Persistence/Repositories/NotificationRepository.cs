using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Persistence.Repositories
{
    public class NotificationRepository : INotificationsRepository
    {
        private readonly AutoTestContext _autoTestContext;

        public NotificationRepository(AutoTestContext autoTestContext)
        {
            _autoTestContext = autoTestContext;
        }

        async Task INotificationsRepository.AddNotification(Notification notification, CancellationToken cancellationToken)
        {
            await _autoTestContext.Notifications!.Upsert(notification, a => a.NotificationId == notification.NotificationId, cancellationToken);
            await this._autoTestContext.SaveChangesAsync(cancellationToken);
        }

        Task<IEnumerable<Notification>> INotificationsRepository.GetNotifications(ulong eventId, CancellationToken cancellationToken)
        {
            return _autoTestContext.Notifications!.Where(a => a.EventId == eventId).OrderByDescending(a => a.Created).ToEnumerableAsync(cancellationToken);
        }
    }
}
