using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;
using UniqueIdGenerator.Net;

namespace AutoTest.Persistence.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly AutoTestContext _autoTestContext;
        private static readonly Generator Generator = new Generator(0, new DateTime(2020, 04, 17));

        public EventsRepository(AutoTestContext autoTestContext)
        {
            _autoTestContext = autoTestContext;
        }

        async Task<Event?> IEventsRepository.GetById(ulong eventId, CancellationToken cancellationToken)
        {
            return await _autoTestContext.Events.Where(a => a.EventId == eventId).SingleOrDefaultAsync(cancellationToken);
        }

        Task<IEnumerable<Event>> IEventsRepository.GetAll(CancellationToken cancellationToken)
        {
            return _autoTestContext.Events.OrderBy(a => a.StartTime).ToEnumerableAsync(cancellationToken);
        }

        async Task IEventsRepository.Upsert(Event @event, CancellationToken cancellationToken)
        {
            await (this._autoTestContext.Events.ThrowIfNull()).Upsert(@event, a => a.EventId == @event.EventId, cancellationToken);
            SyncTests(@event);

            await this._autoTestContext.SaveChangesAsync(cancellationToken);
        }

        private void SyncTests(Event @event)
        {
            //var tests = this._autoTestContext.Tests.Where(a => a.EventId == @event.EventId);
            //var expectedOrdinals = Enumerable.Range(0, @event.TestCount);
            //var toAddOrdinals = expectedOrdinals.Except(tests.Select(a => a.Ordinal));

            //this._autoTestContext.Tests.ThrowIfNull().AddRange(toAddOrdinals.Select(a => new Test(Generator.NextLong(), @event.EventId, a, null)));
            //var toRemove = tests.Where(a => !expectedOrdinals.Contains(a.Ordinal));
            //foreach (var test in toRemove)
            //{
            //    this._autoTestContext.Add(test);
            //    this._autoTestContext.Tests.ThrowIfNull().Remove(test).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            //}
        }
    }
}
