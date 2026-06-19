using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers;

public class GetClubsHandler(IClubsRepository clubRepository) : IRequestHandler<GetClubs, IEnumerable<Club>>
{
    Task<IEnumerable<Club>> IRequestHandler<GetClubs, IEnumerable<Club>>.Handle(GetClubs request, CancellationToken cancellationToken)
    {
        return clubRepository.GetAll(cancellationToken);
    }
}
