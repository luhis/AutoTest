using AutoTest.Domain.Repositories;

namespace AutoTest.Service.Handlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoTest.Domain.StorageModels;
    using AutoTest.Service.Messages;
    using MediatR;

    public class GetAllEventsHandler : IRequestHandler<GetAllEvents, IEnumerable<Event>>
    {
        private readonly IEventsRepository _eventsRepository;

        public GetAllEventsHandler(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        Task<IEnumerable<Event>> IRequestHandler<GetAllEvents, IEnumerable<Event>>.Handle(GetAllEvents request, CancellationToken cancellationToken)
        {
            return this._eventsRepository.GetAll(cancellationToken);
        }
    }
}
