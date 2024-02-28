using AutoTest.Domain.Repositories;

namespace AutoTest.Service.Handlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Domain.StorageModels;
    using AutoTest.Service.Messages;
    using MediatR;

    public class GetAllEventsHandler(IEventsRepository eventsRepository) : IRequestHandler<GetAllEvents, IEnumerable<Event>>
    {
        Task<IEnumerable<Event>> IRequestHandler<GetAllEvents, IEnumerable<Event>>.Handle(GetAllEvents request, CancellationToken cancellationToken)
        {
            return eventsRepository.GetAll(cancellationToken);
        }
    }
}
