namespace AutoTest.Service.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Persistence;
    using AutoTest.Service.Messages;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    using static AutoTest.Service.NonNuller;

    public class DeleteClubHandler : IRequestHandler<DeleteClub>
    {
        private readonly AutoTestContext autoTestContext;

        public DeleteClubHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<Unit> IRequestHandler<DeleteClub, Unit>.Handle(DeleteClub request, CancellationToken cancellationToken)
        {
            var found = await this.autoTestContext.Clubs.SingleAsync(a => a.ClubId == request.ClubId, cancellationToken);
            ThrowIfNull(this.autoTestContext.Clubs).Remove(found);
            await this.autoTestContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
