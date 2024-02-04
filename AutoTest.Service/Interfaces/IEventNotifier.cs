using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Service.Interfaces
{
    public interface IEventNotifier
    {
        Task NewTestRun(TestRun testRun, CancellationToken cancellationToken);
        Task NewNotification(Notification notification, CancellationToken cancellationToken);
        Task EventStatusChanged(ulong eventId, EventStatus newStatus, CancellationToken cancellationToken);
    }
}
