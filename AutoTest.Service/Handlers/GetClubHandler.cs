using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetClubHandler(IClubsRepository clubRepository) : IRequestHandler<GetClub, Club?>
{
    public async ValueTask<Club?> Handle(GetClub request, CancellationToken cancellationToken)
    {
        return await clubRepository.GetById(request.ClubId, cancellationToken);
    }
}
