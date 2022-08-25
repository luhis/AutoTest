using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;
namespace AutoTest.Service.Handlers
{
    public class IsClubAdminHandler : IRequestHandler<IsClubAdmin, bool>
    {
        private readonly IClubsRepository autoTestContext;
        private readonly IEventsRepository eventsRepository;

        public IsClubAdminHandler(IClubsRepository autoTestContext, IEventsRepository eventsRepository)
        {
            this.autoTestContext = autoTestContext;
            this.eventsRepository = eventsRepository;
        }

        async Task<bool> IRequestHandler<IsClubAdmin, bool>.Handle(IsClubAdmin request, CancellationToken cancellationToken)
        {
            var @event = await this.eventsRepository.GetById(request.EventId, cancellationToken);

            var club = await this.autoTestContext.GetById(@event.ClubId, cancellationToken);
            return club != null && club.AdminEmails.Select(a => a.Email).Contains(request.EmailAddress, StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
