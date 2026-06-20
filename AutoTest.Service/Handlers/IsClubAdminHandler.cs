using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class IsClubAdminHandler(IClubsRepository clubsRepository, IEventsRepository eventsRepository) : IRequestHandler<IsClubAdmin, bool>
{
    public async ValueTask<bool> Handle(IsClubAdmin request, CancellationToken cancellationToken)
    {
        var @event = await eventsRepository.GetById(request.EventId, cancellationToken);

        var club = await clubsRepository.GetById(@event!.ClubId, cancellationToken);
        return club is not null && club.AdminEmails.Select(a => a.Email).Contains(request.EmailAddress, StringComparer.InvariantCultureIgnoreCase);
    }
}
