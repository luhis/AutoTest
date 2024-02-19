using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Domain.Repositories
{
    public interface INotificationsRepository
    {
        Task AddNotification(Notification notification, CancellationToken cancellationToken);
        Task<IEnumerable<Notification>> GetNotifications(ulong eventId, CancellationToken cancellationToken);
    }
}
