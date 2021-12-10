// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Service.Handlers
{
    public class IsClubAdminHandler : IRequestHandler<IsClubAdmin, bool>
    {
        private readonly AutoTestContext autoTestContext;

        public IsClubAdminHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<bool> IRequestHandler<IsClubAdmin, bool>.Handle(IsClubAdmin request, CancellationToken cancellationToken)
        {
            var @event = await this.autoTestContext.Events!.SingleAsync(a => a.EventId == request.EventId, cancellationToken);
            var club = await this.autoTestContext.Clubs!.SingleAsync(a => a.ClubId == @event.ClubId, cancellationToken);
            return club != null && club.AdminEmails.Select(a => a.Email).Contains(request.EmailAddress);
        }
    }
}
