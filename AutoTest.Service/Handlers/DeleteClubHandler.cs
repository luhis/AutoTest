using AutoTest.Domain.Repositories;

namespace AutoTest.Service.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Service.Messages;
    using MediatR;

    public class DeleteClubHandler : IRequestHandler<DeleteClub>
    {
        private readonly IClubsRepository clubRepository;

        public DeleteClubHandler(IClubsRepository clubRepository)
        {
            this.clubRepository = clubRepository;
        }

        async Task IRequestHandler<DeleteClub>.Handle(DeleteClub request, CancellationToken cancellationToken)
        {
            await clubRepository.Delete(request.ClubId, cancellationToken);
        }
    }
}
