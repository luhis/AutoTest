using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Persistence.Repositories
{
    public class EntrantsRepository : IEntrantsRepository
    {
        private readonly AutoTestContext _autoTestContext;

        public EntrantsRepository(AutoTestContext autoTestContext)
        {
            _autoTestContext = autoTestContext;
        }

        async Task<Entrant?> IEntrantsRepository.GetById(ulong eventId, ulong entrantId, CancellationToken cancellationToken)
        {
            return await _autoTestContext.Entrants!.Where(a => a.EventId == eventId && a.EntrantId == entrantId).SingleOrDefaultAsync(cancellationToken);
        }

        Task<IEnumerable<Entrant>> IEntrantsRepository.GetByEventId(ulong eventId, CancellationToken cancellationToken)
        {
            return _autoTestContext.Entrants!.Where(a => a.EventId == eventId).ToEnumerableAsync(cancellationToken);
        }

        Task<IEnumerable<Entrant>> IEntrantsRepository.GetAll(ulong eventId, CancellationToken cancellationToken)
        {
            return _autoTestContext.Entrants!.Where(a => a.EventId == eventId).OrderBy(a => a.DriverNumber).ToEnumerableAsync(cancellationToken);
        }

        async Task IEntrantsRepository.Upsert(Entrant entrant, CancellationToken cancellationToken)
        {
            await this._autoTestContext.Entrants!.Upsert(entrant, a => a.EntrantId == entrant.EntrantId, cancellationToken);
            await this._autoTestContext.SaveChangesAsync(cancellationToken);
        }

        async Task IEntrantsRepository.Update(Entrant entrant, CancellationToken cancellationToken)
        {
            this._autoTestContext.Entrants!.Update(entrant);
            await this._autoTestContext.SaveChangesAsync(cancellationToken);
        }

        public Task Delete(Entrant entrant, CancellationToken cancellationToken)
        {
            this._autoTestContext.Entrants!.Remove(entrant);
            return _autoTestContext.SaveChangesAsync(cancellationToken);
        }

        public Task<int> GetEntrantCount(ulong eventId, CancellationToken cancellationToken)
        {
            return this._autoTestContext.Entrants!.Where(a => a.EventId == eventId).CountAsync(cancellationToken);
        }
    }
}
