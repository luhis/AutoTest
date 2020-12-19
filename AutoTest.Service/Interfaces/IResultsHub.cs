using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Service.Interfaces
{
    public interface ISignalRNotifier
    {
        Task NewTestRun(ulong eventId, CancellationToken cancellationToken);
        Task NewNotification(Notification eventId, CancellationToken cancellationToken);
    }
}
