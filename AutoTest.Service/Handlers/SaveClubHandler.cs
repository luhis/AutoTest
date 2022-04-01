using AutoTest.Domain.Repositories;

namespace AutoTest.Service.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Service.Messages;
    using MediatR;

    public class SaveClubHandler : IRequestHandler<SaveClub, ulong>
    {
        private readonly IClubsRepository clubRepository;

        public SaveClubHandler(IClubsRepository clubRepository)
        {
            this.clubRepository = clubRepository;
        }

        async Task<ulong> IRequestHandler<SaveClub, ulong>.Handle(SaveClub request, CancellationToken cancellationToken)
        {
            await clubRepository.Upsert(request.Club, cancellationToken);
            return request.Club.ClubId;
        }
    }
}
