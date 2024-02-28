using AutoTest.Domain.Repositories;

namespace AutoTest.Service.Handlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Domain.StorageModels;
    using AutoTest.Service.Messages;
    using MediatR;

    public class GetClubsHandler(IClubsRepository clubRepository) : IRequestHandler<GetClubs, IEnumerable<Club>>
    {
        Task<IEnumerable<Club>> IRequestHandler<GetClubs, IEnumerable<Club>>.Handle(GetClubs request, CancellationToken cancellationToken)
        {
            return clubRepository.GetAll(cancellationToken);
        }
    }
}
