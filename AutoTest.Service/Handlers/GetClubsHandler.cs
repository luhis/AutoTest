using System.Linq;

namespace AutoTest.Service.Handlers
{
    using System.Collections.Generic;
    using AutoTest.Service.Messages;
    using AutoTest.Domain.StorageModels;
    using MediatR;
    using System.Threading.Tasks;
    using System.Threading;
    using AutoTest.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class GetClubsHandler : IRequestHandler<GetClubs, IEnumerable<Club>>
    {
        private readonly AutoTestContext autoTestContext;

        public GetClubsHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<IEnumerable<Club>> IRequestHandler<GetClubs, IEnumerable<Club>>.Handle(GetClubs request, CancellationToken cancellationToken)
        {
            return await this.autoTestContext.Clubs.OrderBy(a => a.ClubName).ToArrayAsync(cancellationToken);
        }
    }
}
