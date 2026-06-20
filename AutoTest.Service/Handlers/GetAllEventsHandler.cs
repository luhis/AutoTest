using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetAllEventsHandler(IEventsRepository eventsRepository) : IRequestHandler<GetAllEvents, IEnumerable<Event>>
{
    public async ValueTask<IEnumerable<Event>> Handle(GetAllEvents request, CancellationToken cancellationToken)
    {
        return await eventsRepository.GetAll(cancellationToken);
    }
}
