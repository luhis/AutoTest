using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetEntrantsHandler(IEntrantsRepository entrantsRepository) : IRequestHandler<GetEntrants, IEnumerable<Entrant>>
{
    public async ValueTask<IEnumerable<Entrant>> Handle(GetEntrants request, CancellationToken cancellationToken)
    {
        return await entrantsRepository.GetAll(request.EventId, cancellationToken);
    }
}
