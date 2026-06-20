using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetTestsHandler(IEventsRepository eventsRepository) : IRequestHandler<GetTests, IEnumerable<Course>>
{
    public async ValueTask<IEnumerable<Course>> Handle(GetTests request, CancellationToken cancellationToken)
    {
        var @event = await eventsRepository.GetById(request.EventId, cancellationToken);

        return @event!.Courses.OrderBy(a => a.Ordinal);
    }
}
