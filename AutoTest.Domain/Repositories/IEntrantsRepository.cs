using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Domain.Repositories
{
    public interface IEntrantsRepository
    {
        Task<Entrant?> GetById(ulong entrantId, CancellationToken cancellationToken);

        Task<IEnumerable<Entrant>> GetAll(ulong eventId, CancellationToken cancellationToken);

        Task Upsert(Entrant entrant, CancellationToken cancellationToken);
    }
}
