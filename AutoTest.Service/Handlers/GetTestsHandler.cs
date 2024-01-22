using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetTestsHandler : IRequestHandler<GetTests, IEnumerable<Test>>
    {
        private readonly IEventsRepository eventsRepository;

        public GetTestsHandler(IEventsRepository eventsRepository)
        {
            this.eventsRepository = eventsRepository;
        }

        async Task<IEnumerable<Test>> IRequestHandler<GetTests, IEnumerable<Test>>.Handle(GetTests request, CancellationToken cancellationToken)
        {
            var @event = await this.eventsRepository.GetById(request.EventId, cancellationToken);

            return @event!.Tests.OrderBy(a => a.Ordinal);
        }
    }
}
