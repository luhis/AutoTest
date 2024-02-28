using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class MarkPaidHandler(IEntrantsRepository entrantsRepository) : IRequestHandler<MarkPaid>
    {
        async Task IRequestHandler<MarkPaid>.Handle(MarkPaid request, CancellationToken cancellationToken)
        {
            var found = (await entrantsRepository.GetById(request.EventId, request.EntrantId, cancellationToken))!;// await this._autoTestContext.Entrants!.SingleAsync(a => a.EventId == request.EventId && a.EntrantId == request.EntrantId, cancellationToken);
            found.SetPayment(request.Payment);
            await entrantsRepository.Update(found, cancellationToken);
        }
    }
}
