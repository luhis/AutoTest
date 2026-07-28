using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetEntrantHandler(IEntrantsRepository entrantsRepository) : IRequestHandler<GetEntrant, Entrant?>
{
    public ValueTask<Entrant?> Handle(GetEntrant request, CancellationToken cancellationToken) =>
        new(entrantsRepository.GetById(request.EventId, request.EntrantId, cancellationToken));
}
