using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SaveEntrantHandler : IRequestHandler<SaveEntrant, ulong>
    {
        private readonly IEntrantsRepository entrantsRepository;

        public SaveEntrantHandler(IEntrantsRepository entrantsRepository)
        {
            this.entrantsRepository = entrantsRepository;
        }

        async Task<ulong> IRequestHandler<SaveEntrant, ulong>.Handle(SaveEntrant request, CancellationToken cancellationToken)
        {
            await entrantsRepository.Upsert(request.Entrant, cancellationToken);
            return request.Entrant.EntrantId;
        }
    }
}
