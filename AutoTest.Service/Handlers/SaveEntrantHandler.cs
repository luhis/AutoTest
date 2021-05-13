using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SaveEntrantHandler : IRequestHandler<SaveEntrant, Entrant>
    {
        private readonly IEntrantsRepository entrantsRepository;

        public SaveEntrantHandler(IEntrantsRepository entrantsRepository)
        {
            this.entrantsRepository = entrantsRepository;
        }

        async Task<Entrant> IRequestHandler<SaveEntrant, Entrant>.Handle(SaveEntrant request, CancellationToken cancellationToken)
        {
            await entrantsRepository.Upsert(request.Entrant, cancellationToken);
            return request.Entrant;
        }
    }
}
