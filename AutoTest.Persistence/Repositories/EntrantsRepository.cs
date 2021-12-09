using System.Collections.Generic;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        async Task<Entrant?> IEntrantsRepository.GetById(ulong entrantId, CancellationToken cancellationToken)
        {
            return await _autoTestContext.Entrants!.Where(a => a.EntrantId == entrantId).SingleOrDefaultAsync(cancellationToken);
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
            await this._autoTestContext.Entrants.ThrowIfNull().Upsert(entrant, a => a.EntrantId == entrant.EntrantId, cancellationToken);
            await this._autoTestContext.SaveChangesAsync(cancellationToken);
        }
    }
}
