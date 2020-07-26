using AutoTest.Domain.Repositories;

namespace AutoTest.Service.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Service.Messages;
    using MediatR;

    public class DeleteClubHandler : IRequestHandler<DeleteClub>
    {
        private readonly IClubRepository clubRepository;

        public DeleteClubHandler(IClubRepository clubRepository)
        {
            this.clubRepository = clubRepository;
        }

        async Task<Unit> IRequestHandler<DeleteClub, Unit>.Handle(DeleteClub request, CancellationToken cancellationToken)
        {
            await clubRepository.Delete(request.ClubId, cancellationToken);
            return Unit.Value;
        }
    }
}
