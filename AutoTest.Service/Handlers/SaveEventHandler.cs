using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;
using UniqueIdGenerator.Net;

namespace AutoTest.Service.Handlers
{
    public class SaveEventHandler : IRequestHandler<SaveEvent, ulong>
    {
        private readonly AutoTestContext autoTestContext;
        private static readonly Generator Generator = new Generator(0, new DateTime(2020, 04, 17));

        public SaveEventHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<ulong> IRequestHandler<SaveEvent, ulong>.Handle(SaveEvent request, CancellationToken cancellationToken)
        {
            if (this.autoTestContext.Events == null)
            {
                throw new NoNullAllowedException(nameof(this.autoTestContext.Clubs));
            }
            await this.autoTestContext.Events.Upsert(request.Event, a => a.EventId == request.Event.EventId, cancellationToken);
            SyncTests(request.Event);

            await this.autoTestContext.SaveChangesAsync(cancellationToken);
            return request.Event.ClubId;
        }

        private void SyncTests(Event @event)
        {
            var tests = this.autoTestContext.Tests.Where(a => a.EventId == @event.EventId);
            var expectedOrdinals = Enumerable.Range(1, @event.TestCount);
            var toAddOrdinals = expectedOrdinals.Except(tests.Select(a => a.Ordinal));

            this.autoTestContext.Tests.AddRange(toAddOrdinals.Select(a => new Test(Generator.NextLong(), @event.EventId, a, null)));
            var toRemove = tests.Where(a => !expectedOrdinals.Contains(a.Ordinal));
            foreach (var test in toRemove)
            {
                this.autoTestContext.Add(test);
                this.autoTestContext.Tests.Remove(test).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
        }
    }
}
