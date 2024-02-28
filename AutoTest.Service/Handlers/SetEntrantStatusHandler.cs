using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SetEntrantStatusHandler(IEntrantsRepository entrantsRepository) : IRequestHandler<SetEntrantStatus>
    {
        async Task IRequestHandler<SetEntrantStatus>.Handle(SetEntrantStatus request, CancellationToken cancellationToken)
        {
            var entrant = await entrantsRepository.GetById(request.EventId, request.EntrantId, cancellationToken);
            entrant!.SetEntrantStatus(request.Status);
            await entrantsRepository.Upsert(entrant, cancellationToken);
        }
    }
}
