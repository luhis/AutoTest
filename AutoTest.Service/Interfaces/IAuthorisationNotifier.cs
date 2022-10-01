using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTest.Service.Interfaces
{
    public interface IAuthorisationNotifier
    {

        Task NewClubAdmin(ulong clubId, IEnumerable<string> newEmails);
        Task RemoveClubAdmin(ulong clubId, IEnumerable<string> newEmails);
        Task NewEventMarshal(ulong eventId, IEnumerable<string> newEmails, CancellationToken cancellationToken);
        Task RemoveEventMarshal(ulong eventId, IEnumerable<string> newEmails, CancellationToken cancellationToken);
    }
}
