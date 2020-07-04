using System.Linq;

namespace AutoTest.Service.Handlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Domain.StorageModels;
    using AutoTest.Persistence;
    using AutoTest.Service.Messages;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetAllEventsHandler : IRequestHandler<GetAllEvents, IEnumerable<Event>>
    {
        private readonly AutoTestContext autoTestContext;

        public GetAllEventsHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        public async Task<IEnumerable<Event>> Handle(GetAllEvents request, CancellationToken cancellationToken)
        {
            return await this.autoTestContext.Events.ToArrayAsync(cancellationToken);
        }
    }
}
