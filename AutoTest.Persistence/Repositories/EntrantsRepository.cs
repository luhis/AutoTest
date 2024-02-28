using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Persistence.Repositories
{
    public class EntrantsRepository(AutoTestContext autoTestContext) : IEntrantsRepository
    {
        async Task<Entrant?> IEntrantsRepository.GetById(ulong eventId, ulong entrantId, CancellationToken cancellationToken)
        {
            return await autoTestContext.Entrants!.Where(a => a.EventId == eventId && a.EntrantId == entrantId).SingleOrDefaultAsync(cancellationToken);
        }

        Task<IEnumerable<Entrant>> IEntrantsRepository.GetByEventId(ulong eventId, CancellationToken cancellationToken)
        {
            return autoTestContext.Entrants!.Where(a => a.EventId == eventId).ToEnumerableAsync(cancellationToken);
        }

        Task<IEnumerable<Entrant>> IEntrantsRepository.GetAll(ulong eventId, CancellationToken cancellationToken)
        {
            return autoTestContext.Entrants!.Where(a => a.EventId == eventId).OrderBy(a => a.DriverNumber).ToEnumerableAsync(cancellationToken);
        }

        async Task IEntrantsRepository.Upsert(Entrant entrant, CancellationToken cancellationToken)
        {
            await autoTestContext.Entrants!.Upsert(entrant, a => a.EntrantId == entrant.EntrantId, cancellationToken);
            await autoTestContext.SaveChangesAsync(cancellationToken);
        }

        async Task IEntrantsRepository.Update(Entrant entrant, CancellationToken cancellationToken)
        {
            autoTestContext.Entrants!.Update(entrant);
            await autoTestContext.SaveChangesAsync(cancellationToken);
        }

        public Task Delete(Entrant entrant, CancellationToken cancellationToken)
        {
            autoTestContext.Entrants!.Remove(entrant);
            return autoTestContext.SaveChangesAsync(cancellationToken);
        }

        public Task<int> GetEntrantCount(ulong eventId, CancellationToken cancellationToken)
        {
            return autoTestContext.Entrants!.Where(a => a.EventId == eventId).CountAsync(cancellationToken);
        }
    }
}
