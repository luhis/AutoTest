using AutoTest.Domain.Repositories;

namespace AutoTest.Service.Handlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Domain.StorageModels;
    using AutoTest.Service.Messages;
    using MediatR;

    public class GetClubsHandler : IRequestHandler<GetClubs, IEnumerable<Club>>
    {
        private readonly IClubsRepository clubRepository;

        public GetClubsHandler(IClubsRepository clubRepository)
        {
            this.clubRepository = clubRepository;
        }

        Task<IEnumerable<Club>> IRequestHandler<GetClubs, IEnumerable<Club>>.Handle(GetClubs request, CancellationToken cancellationToken)
        {
            return clubRepository.GetAll(cancellationToken);
        }
    }
}
