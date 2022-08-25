using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Domain.Repositories
{
    public interface IMarshalsRepository
    {
        Task<ulong> GetMashalIdByEmail(ulong eventId, string emailAddress, CancellationToken cancellationToken);
        Task<Marshal?> GetById(ulong eventId, ulong marshalId, CancellationToken cancellationToken);
        IQueryable<Marshal> GetByEmail(string emailAddress);
        IQueryable<Marshal> GetByEventId(ulong eventId);
        Task Upsert(Marshal marshal, CancellationToken cancellationToken);
        Task Remove(Marshal marshal, CancellationToken cancellationToken);
    }
}
