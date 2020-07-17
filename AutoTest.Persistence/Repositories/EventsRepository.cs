using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Persistence.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly AutoTestContext _autoTestContext;

        public EventsRepository(AutoTestContext autoTestContext)
        {
            _autoTestContext = autoTestContext;
        }

        async Task<Event?> IEventsRepository.GetById(ulong eventId, CancellationToken cancellationToken)
        {
            return await _autoTestContext.Events.Where(a => a.EventId == eventId).SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Event>> GetAll(CancellationToken cancellationToken)
        {
            return await _autoTestContext.Events.OrderBy(a => a.StartTime).ToArrayAsync(cancellationToken);
        }
    }
}
