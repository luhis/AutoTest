using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers;

public class GetAllEventsHandler(IEventsRepository eventsRepository) : IRequestHandler<GetAllEvents, IEnumerable<Event>>
{
    Task<IEnumerable<Event>> IRequestHandler<GetAllEvents, IEnumerable<Event>>.Handle(GetAllEvents request, CancellationToken cancellationToken)
    {
        return eventsRepository.GetAll(cancellationToken);
    }
}
