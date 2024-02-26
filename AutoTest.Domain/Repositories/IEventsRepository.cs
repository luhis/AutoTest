using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Domain.Repositories
{
    public interface IEventsRepository
    {
        Task<Event?> GetById(ulong eventId, CancellationToken cancellationToken);

        Task<IEnumerable<Event>> GetAll(CancellationToken cancellationToken);

        Task Upsert(Event evnt, CancellationToken cancellationToken);

        Task Delete(Event evnt, CancellationToken cancellationToken);
    }
}
