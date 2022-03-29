using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Service.Handlers
{
    public class IsClubAdminHandler : IRequestHandler<IsClubAdmin, bool>
    {
        private readonly AutoTestContext autoTestContext;
        private readonly IEventsRepository eventsRepository;

        public IsClubAdminHandler(AutoTestContext autoTestContext, IEventsRepository eventsRepository)
        {
            this.autoTestContext = autoTestContext;
            this.eventsRepository = eventsRepository;
        }

        async Task<bool> IRequestHandler<IsClubAdmin, bool>.Handle(IsClubAdmin request, CancellationToken cancellationToken)
        {
            var @event = await this.eventsRepository.GetById(request.EventId, cancellationToken);

            var club = await this.autoTestContext.Clubs!.SingleAsync(a => a.ClubId == @event.ClubId, cancellationToken);
            return club != null && club.AdminEmails.Select(a => a.Email).Contains(request.EmailAddress);
        }
    }
}
