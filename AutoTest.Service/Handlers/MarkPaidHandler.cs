using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Service.Handlers
{
    public class MarkPaidHandler : IRequestHandler<MarkPaid>
    {
        private readonly AutoTestContext _autoTestContext;

        public MarkPaidHandler(AutoTestContext autoTestContext)
        {
            _autoTestContext = autoTestContext;
        }


        async Task<Unit> IRequestHandler<MarkPaid, Unit>.Handle(MarkPaid request, CancellationToken cancellationToken)
        {
            var found = await this._autoTestContext.Entrants!.SingleAsync(a => a.EventId == request.EventId && a.EntrantId == request.EntrantId, cancellationToken);
            found.SetPayment(request.Payment);
            this._autoTestContext.Entrants!.Update(found);
            await this._autoTestContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
