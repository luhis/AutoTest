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
        private readonly ISignalRNotifier signalRNotifier;

        public SaveClubHandler(IClubsRepository clubRepository, ISignalRNotifier signalRNotifier)
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
                    await signalRNotifier.NewClubAdmin(request.Club.ClubId, newEmails);
                }
            }
            return request.Club.ClubId;
        }

        private static IEnumerable<string> GetNewItems(IEnumerable<string> oldValues, IEnumerable<string> newValues)
        {
            return newValues.Where(v => !oldValues.Any(ov => ov.Equals(v, StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}
