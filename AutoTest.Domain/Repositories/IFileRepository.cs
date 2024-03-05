using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTest.Domain.Repositories
{
    public interface IFileRepository
    {
        Task<string> GetRegs(ulong eventId, CancellationToken cancellationToken);
        Task SaveRegs(ulong eventId, string data, CancellationToken cancellationToken);
        Task DeleteRegs(ulong eventId, CancellationToken cancellationToken);
        Task<string> GetMaps(ulong eventId, CancellationToken cancellationToken);
        Task SaveMaps(ulong eventId, string data, CancellationToken cancellationToken);
        Task DeleteMaps(ulong eventId, CancellationToken cancellationToken);
    }
}
