using AutoTest.Domain.Repositories;

namespace AutoTest.Service.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Service.Interfaces;
    using AutoTest.Service.Messages;
    using MediatR;

    public class SaveClubHandler : IRequestHandler<SaveClub, ulong>
    {
        private readonly IClubsRepository clubRepository;
        private readonly IAuthorisationNotifier signalRNotifier;

        public SaveClubHandler(IClubsRepository clubRepository, IAuthorisationNotifier signalRNotifier)
        {
            this.clubRepository = clubRepository;
            this.signalRNotifier = signalRNotifier;
        }

        async Task<ulong> IRequestHandler<SaveClub, ulong>.Handle(SaveClub request, CancellationToken cancellationToken)
        {
            var existing = await clubRepository.GetById(request.Club.ClubId, cancellationToken);
            await clubRepository.Upsert(request.Club, cancellationToken);
            if (existing != null && existing.AdminEmails != request.Club.AdminEmails)
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
            return newValues.Where(v => !oldValues.Any(ov => ov.Equals(v, StringComparison.InvariantCultureIgnoreCase)));
        }

        private static IEnumerable<string> GetRemovedItems(IEnumerable<string> oldValues, IEnumerable<string> newValues)
        {
            return oldValues.Where(v => !newValues.Any(ov => ov.Equals(v, StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}
