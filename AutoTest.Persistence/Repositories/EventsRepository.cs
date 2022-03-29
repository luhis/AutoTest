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

        async Task<Event> IEventsRepository.GetById(ulong eventId, CancellationToken cancellationToken)
        {
            return await _autoTestContext.Events!.Where(a => a.EventId == eventId).SingleAsync(cancellationToken);
        }

        Task<IEnumerable<Event>> IEventsRepository.GetAll(CancellationToken cancellationToken)
        {
            return _autoTestContext.Events!.OrderBy(a => a.StartTime).ToEnumerableAsync(cancellationToken);
        }

        async Task IEventsRepository.Upsert(Event @event, CancellationToken cancellationToken)
        {
            SyncTests(@event);
            await this._autoTestContext.Events!.Upsert(@event, a => a.EventId == @event.EventId, cancellationToken);

            await this._autoTestContext.SaveChangesAsync(cancellationToken);
        }

        private static void SyncTests(Event @event)
        {
            var tests = @event.Tests;
            var expectedOrdinals = Enumerable.Range(0, @event.TestCount).ToArray();
            var toAddOrdinals = expectedOrdinals.Except(tests.Select(a => a.Ordinal));

            @event.SetTests(tests.Where(a => expectedOrdinals.Contains(a.Ordinal)).Concat(toAddOrdinals.Select(a => new Test(a, ""))).ToArray());
        }

        Task IEventsRepository.Delete(Event @event, CancellationToken cancellationToken)
        {
            this._autoTestContext.Events!.Remove(@event);
            return this._autoTestContext.SaveChangesAsync();
        }
    }
}
