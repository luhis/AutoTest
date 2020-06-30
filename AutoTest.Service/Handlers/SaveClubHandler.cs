using System.Data;

namespace AutoTest.Service.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Persistence;
    using AutoTest.Service.Messages;
    using MediatR;

    public class SaveClubHandler : IRequestHandler<SaveClub, ulong>
    {
        private readonly AutoTestContext autoTestContext;

        public SaveClubHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<ulong> IRequestHandler<SaveClub, ulong>.Handle(SaveClub request, CancellationToken cancellationToken)
        {
            if (this.autoTestContext.Clubs == null)
            {
                throw new NoNullAllowedException(nameof(this.autoTestContext.Clubs));
            }
            await this.autoTestContext.Clubs.Upsert(request.Club, a => a.ClubId == request.Club.ClubId, cancellationToken);
            await this.autoTestContext.SaveChangesAsync(cancellationToken);
            return request.Club.ClubId;
        }
    }
}
