using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SetEntrantStatusHandler : IRequestHandler<SetEntrantStatus>
    {
        private readonly IEntrantsRepository _entrantsRepository;

        public SetEntrantStatusHandler(IEntrantsRepository entrantsRepository)
        {
            _entrantsRepository = entrantsRepository;
        }

        async Task<Unit> IRequestHandler<SetEntrantStatus, Unit>.Handle(SetEntrantStatus request, CancellationToken cancellationToken)
        {
            var entrant = await _entrantsRepository.GetById(request.EventId, request.EntrantId, cancellationToken);
            entrant!.SetEntrantStatus(request.Status);
            await _entrantsRepository.Upsert(entrant, cancellationToken);
            return new Unit();
        }
    }
}
