using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetAdminClubsHandler : IRequestHandler<GetAdminClubs, IEnumerable<ulong>>
    {
        private readonly AutoTestContext autoTestContext;

        public GetAdminClubsHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<IEnumerable<ulong>> IRequestHandler<GetAdminClubs, IEnumerable<ulong>>.Handle(GetAdminClubs request, CancellationToken cancellationToken)
        {
            // todo this is very ineficcient, but the only other option at this time is SQL
            return (await autoTestContext.Clubs!.ToEnumerableAsync(cancellationToken)).Where(a => a.AdminEmails.Any(e => e.Email == request.EmailAddress)).Select(a => a.ClubId).Distinct();
        }
    }
}
