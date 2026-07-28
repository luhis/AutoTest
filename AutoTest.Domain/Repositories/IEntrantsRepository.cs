using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Domain.Repositories;

public interface IEntrantsRepository
{
    Task<Entrant?> GetById(ulong eventId, ulong entrantId, CancellationToken cancellationToken);

    Task<IEnumerable<Entrant>> GetAll(ulong eventId, CancellationToken cancellationToken);

    Task<IEnumerable<ulong>> GetEntrantIdsByEmail(string email, CancellationToken cancellationToken);

    Task Upsert(Entrant entrant, CancellationToken cancellationToken);

    Task Update(Entrant entrant, CancellationToken cancellationToken);

    Task Delete(Entrant entrant, CancellationToken cancellationToken);

    Task<int> GetEntrantCount(ulong eventId, CancellationToken cancellationToken);
}
