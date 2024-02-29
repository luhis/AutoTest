using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTest.Domain.Repositories
{
    public interface IFileRepository
    {
        Task<string> GetRegs(ulong eventId, CancellationToken cancellationToken);
        Task<string> SaveRegs(ulong eventId, string data, CancellationToken cancellationToken);
        Task<string> GetMaps(ulong eventId, CancellationToken cancellationToken);
        Task<string> SaveMaps(ulong eventId, string data, CancellationToken cancellationToken);
    }
}
