using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Service.Handlers
{
    public class DeleteEntrantHandler : IRequestHandler<DeleteEntrant>
    {
        private readonly AutoTestContext _autoTestContext;

        public DeleteEntrantHandler(AutoTestContext autoTestContext)
        {
            _autoTestContext = autoTestContext;
        }


        async Task<Unit> IRequestHandler<DeleteEntrant, Unit>.Handle(DeleteEntrant request, CancellationToken cancellationToken)
        {
            var found = await this._autoTestContext.Entrants!.SingleAsync(a => a.EntrantId == request.EntrantId, cancellationToken);
            this._autoTestContext.Entrants!.Remove(found);
            await this._autoTestContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
