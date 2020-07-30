using System.Threading;
using System.Threading.Tasks;

namespace AutoTest.Service.Interfaces
{
    public interface ISignalRNotifier
    {
        Task NewTestRun(ulong eventId, CancellationToken cancellationToken);
    }
}
