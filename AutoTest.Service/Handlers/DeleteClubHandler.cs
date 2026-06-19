using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers;

public class DeleteClubHandler(IClubsRepository clubRepository) : IRequestHandler<DeleteClub>
{
    async Task IRequestHandler<DeleteClub>.Handle(DeleteClub request, CancellationToken cancellationToken)
    {
        await clubRepository.Delete(request.ClubId, cancellationToken);
    }
}
