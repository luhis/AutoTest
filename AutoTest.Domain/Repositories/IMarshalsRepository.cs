using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Domain.Repositories
{
    public interface IMarshalsRepository
    {
        Task<Marshal> GetById(ulong eventId, string emailAddress, CancellationToken cancellationToken);
        Task<IEnumerable<Marshal>> GetByEventId(ulong eventId, CancellationToken cancellationToken);
    }
}
