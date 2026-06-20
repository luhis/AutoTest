using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class SaveClubHandler(IClubsRepository clubRepository, IAuthorisationNotifier signalRNotifier) : IRequestHandler<SaveClub, ulong>
{
    public async ValueTask<ulong> Handle(SaveClub request, CancellationToken cancellationToken)
    {
        var existing = await clubRepository.GetById(request.Club.ClubId, cancellationToken);
        await clubRepository.Upsert(request.Club, cancellationToken);
        if (existing is not null && existing.AdminEmails != request.Club.AdminEmails)
        {
            var newEmails = GetNewItems(existing.AdminEmails.Select(a => a.Email), request.Club.AdminEmails.Select(a => a.Email));
            if (newEmails.Any())
            {
                await signalRNotifier.NewClubAdmin(request.Club.ClubId, newEmails, cancellationToken);
            }
            var removedEmails = GetRemovedItems(existing.AdminEmails.Select(a => a.Email), request.Club.AdminEmails.Select(a => a.Email));
            if (removedEmails.Any())
            {
                await signalRNotifier.RemoveClubAdmin(request.Club.ClubId, removedEmails, cancellationToken);
            }
        }
        return request.Club.ClubId;
    }

    private static IEnumerable<string> GetNewItems(IEnumerable<string> oldValues, IEnumerable<string> newValues)
    {
        return newValues.Where(v => !oldValues.Any(ov => ov.Equals(v, StringComparison.OrdinalIgnoreCase)));
    }

    private static IEnumerable<string> GetRemovedItems(IEnumerable<string> oldValues, IEnumerable<string> newValues)
    {
        return oldValues.Where(v => !newValues.Any(ov => ov.Equals(v, StringComparison.OrdinalIgnoreCase)));
    }
}
