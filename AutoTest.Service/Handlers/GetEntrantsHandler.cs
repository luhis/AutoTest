using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetEntrantsHandler(IEntrantsRepository entrantsRepository) : IRequestHandler<GetEntrants, IEnumerable<Entrant>>
    {
        Task<IEnumerable<Entrant>> IRequestHandler<GetEntrants, IEnumerable<Entrant>>.Handle(GetEntrants request, CancellationToken cancellationToken)
        {
            return entrantsRepository.GetAll(request.EventId, cancellationToken);
        }
    }
}
