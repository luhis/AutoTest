using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetClubsHandler(IClubsRepository clubRepository) : IRequestHandler<GetClubs, IEnumerable<Club>>
{
    public async ValueTask<IEnumerable<Club>> Handle(GetClubs request, CancellationToken cancellationToken)
    {
        return await clubRepository.GetAll(cancellationToken);
    }
}
