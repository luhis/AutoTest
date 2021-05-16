using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetEntrantsHandler : IRequestHandler<GetEntrants, IEnumerable<Entrant>>
    {
        private readonly IEntrantsRepository entrantsRepository;

        public GetEntrantsHandler(IEntrantsRepository entrantsRepository)
        {
            this.entrantsRepository = entrantsRepository;
        }

        Task<IEnumerable<Entrant>> IRequestHandler<GetEntrants, IEnumerable<Entrant>>.Handle(GetEntrants request, CancellationToken cancellationToken)
        {
            return this.entrantsRepository.GetAll(request.EventId, cancellationToken);
        }
    }
}
