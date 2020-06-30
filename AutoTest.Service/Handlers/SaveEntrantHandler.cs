using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SaveEntrantHandler : IRequestHandler<SaveEntrant, ulong>
    {
        private readonly AutoTestContext autoTestContext;

        public SaveEntrantHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<ulong> IRequestHandler<SaveEntrant, ulong>.Handle(SaveEntrant request, CancellationToken cancellationToken)
        {
            if (this.autoTestContext.Entrants == null)
            {
                throw new NoNullAllowedException(nameof(this.autoTestContext.Clubs));
            }
            await this.autoTestContext.Entrants.Upsert(request.Entrant, a => a.EventId == request.Entrant.EntrantId, cancellationToken);
            await this.autoTestContext.SaveChangesAsync(cancellationToken);
            return request.Entrant.EntrantId;
        }
    }
}
