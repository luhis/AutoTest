using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Service.Handlers
{
    public class GetEntrantsHandler : IRequestHandler<GetEntrants, IEnumerable<Entrant>>
    {
        private readonly AutoTestContext autoTestContext;

        public GetEntrantsHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<IEnumerable<Entrant>> IRequestHandler<GetEntrants, IEnumerable<Entrant>>.Handle(GetEntrants request, CancellationToken cancellationToken)
        {
            return await this.autoTestContext.Entrants.Where(a => a.EventId == request.EventId).ToArrayAsync(cancellationToken);
        }
    }
}
