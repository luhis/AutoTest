using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class SetEntrantStatusHandler(IEntrantsRepository entrantsRepository) : IRequestHandler<SetEntrantStatus>
{
    public async ValueTask<Unit> Handle(SetEntrantStatus request, CancellationToken cancellationToken)
    {
        var entrant = await entrantsRepository.GetById(request.EventId, request.EntrantId, cancellationToken);
        entrant!.SetEntrantStatus(request.Status);
        await entrantsRepository.Upsert(entrant, cancellationToken);
        return Unit.Value;
    }
}
