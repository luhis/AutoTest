﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace AutoTest.Service.Handlers
{
    public class GetAdminClubsHandler(IClubsRepository clubsRepository, IMemoryCache cache) : IRequestHandler<GetAdminClubs, IEnumerable<ulong>>
    {
        async Task<IEnumerable<ulong>> IRequestHandler<GetAdminClubs, IEnumerable<ulong>>.Handle(GetAdminClubs request, CancellationToken cancellationToken)
        {
            // todo this is very ineficcient, but the only other option at this time is SQL
            var clubAdminEmails = await GetOrCreate(cancellationToken);
            return clubAdminEmails.Where(a => a.AdminEmails.Any(e => e.Email == request.EmailAddress)).Select(a => a.ClubId).Distinct();
        }

        private static readonly string cacheKey = nameof(GetAdminClubsHandler);

        private async Task<IEnumerable<(ulong ClubId, IEnumerable<AuthorisationEmail> AdminEmails)>> GetOrCreate(CancellationToken cancellationToken)
        {
            if (cache.TryGetValue<IEnumerable<(ulong ClubId, IEnumerable<AuthorisationEmail> AdminEmails)>>(cacheKey, out var o) && o != null)
            {
                return o;
            }
            else
            {
                var r = await GetClubAdminEmails(cancellationToken);
                cache.Set(cacheKey, r, TimeSpan.FromSeconds(30));
                return r;
            }
        }

        private async Task<IEnumerable<(ulong ClubId, IEnumerable<AuthorisationEmail> AdminEmails)>> GetClubAdminEmails(CancellationToken cancellationToken) =>
            (await clubsRepository.GetAll(cancellationToken)).Select(a => (a.ClubId, a.AdminEmails.AsEnumerable()));
    }
}
