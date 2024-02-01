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
    public class GetTestsHandler : IRequestHandler<GetTests, IEnumerable<Course>>
    {
        private readonly IEventsRepository eventsRepository;

        public GetTestsHandler(IEventsRepository eventsRepository)
        {
            this.eventsRepository = eventsRepository;
        }

        async Task<IEnumerable<Course>> IRequestHandler<GetTests, IEnumerable<Course>>.Handle(GetTests request, CancellationToken cancellationToken)
        {
            var @event = await this.eventsRepository.GetById(request.EventId, cancellationToken);

            return @event!.Courses.OrderBy(a => a.Ordinal);
        }
    }
}
