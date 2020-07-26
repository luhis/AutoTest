using AutoTest.Domain.Repositories;

namespace AutoTest.Service.Handlers
{
    using System.Collections.Generic;
    using AutoTest.Service.Messages;
    using AutoTest.Domain.StorageModels;
    using MediatR;
    using System.Threading.Tasks;
    using System.Threading;

    public class GetClubsHandler : IRequestHandler<GetClubs, IEnumerable<Club>>
    {
        private readonly IClubRepository clubRepository;

        public GetClubsHandler(IClubRepository clubRepository)
        {
            this.clubRepository = clubRepository;
        }

        Task<IEnumerable<Club>> IRequestHandler<GetClubs, IEnumerable<Club>>.Handle(GetClubs request, CancellationToken cancellationToken)
        {
            return clubRepository.GetAll(cancellationToken);
        }
    }
}
