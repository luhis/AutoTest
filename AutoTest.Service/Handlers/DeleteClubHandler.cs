using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class DeleteClubHandler(IClubsRepository clubRepository) : IRequestHandler<DeleteClub>
{
    public async ValueTask<Unit> Handle(DeleteClub request, CancellationToken cancellationToken)
    {
        await clubRepository.Delete(request.ClubId, cancellationToken);
        return Unit.Value;
    }
}
