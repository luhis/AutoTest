using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Service.Handlers
{
    public class DeleteEventHandler : IRequestHandler<DeleteEvent>
    {
        private readonly AutoTestContext _autoTestContext;

        public DeleteEventHandler(AutoTestContext autoTestContext)
        {
            _autoTestContext = autoTestContext;
        }

        async Task<Unit> IRequestHandler<DeleteEvent, Unit>.Handle(DeleteEvent request, CancellationToken cancellationToken)
        {
            var found = await this._autoTestContext.Events!.SingleAsync(a => a.EventId == request.EventId, cancellationToken);
            this._autoTestContext.Events!.Remove(found);
            await this._autoTestContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
