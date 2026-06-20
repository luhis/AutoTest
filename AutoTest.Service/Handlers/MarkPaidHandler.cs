using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class MarkPaidHandler(IEntrantsRepository entrantsRepository) : IRequestHandler<MarkPaid>
{
    public async ValueTask<Unit> Handle(MarkPaid request, CancellationToken cancellationToken)
    {
        var found = (await entrantsRepository.GetById(request.EventId, request.EntrantId, cancellationToken))!;
        found.SetPayment(request.Payment);
        await entrantsRepository.Update(found, cancellationToken);
        return Unit.Value;
    }
}
