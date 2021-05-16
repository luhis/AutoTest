using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Service.Handlers
{
    public class DeleteMarshalHandler : IRequestHandler<DeleteMarshal>
    {
        private readonly AutoTestContext _autoTestContext;

        public DeleteMarshalHandler(AutoTestContext autoTestContext)
        {
            _autoTestContext = autoTestContext;
        }

        async Task<Unit> IRequestHandler<DeleteMarshal, Unit>.Handle(DeleteMarshal request, CancellationToken cancellationToken)
        {
            var found = await this._autoTestContext.Marshals!.SingleAsync(a => a.MarshalId == request.MarshalId, cancellationToken);
            this._autoTestContext.Marshals!.Remove(found);
            await this._autoTestContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
