using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetEntrantHandler(IEntrantsRepository entrantsRepository) : IRequestHandler<GetEntrant, Entrant?>
{
    public async ValueTask<Entrant?> Handle(GetEntrant request, CancellationToken cancellationToken)
    {
        return await entrantsRepository.GetById(request.EventId, request.EntrantId, cancellationToken);
    }
}
