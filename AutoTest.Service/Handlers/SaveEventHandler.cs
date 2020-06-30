using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SaveEventHandler : IRequestHandler<SaveEvent, ulong>
    {
        private readonly AutoTestContext autoTestContext;

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
            await this.autoTestContext.Events.Upsert(request.Event, a => a.EventId == request.Event.ClubId, cancellationToken);
            await this.autoTestContext.SaveChangesAsync(cancellationToken);
            return request.Event.ClubId;
        }
    }
}
