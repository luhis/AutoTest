using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Service.Interfaces
{
    public interface ISignalRNotifier
    {
        Task NewTestRun(TestRun testRun, CancellationToken cancellationToken);
        Task NewNotification(Notification notification, CancellationToken cancellationToken);
        Task NewClubAdmin(ulong clubId, IEnumerable<string> newEmails);
        Task RemoveClubAdmin(ulong clubId, IEnumerable<string> newEmails);
        Task NewEventMarshal(ulong eventId, IEnumerable<string> newEmails, CancellationToken cancellationToken);
        Task RemoveEventMarshal(ulong eventId, IEnumerable<string> newEmails, CancellationToken cancellationToken);
    }
}
