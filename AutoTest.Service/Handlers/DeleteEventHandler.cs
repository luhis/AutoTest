using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class DeleteEventHandler : IRequestHandler<DeleteEvent>
    {
        private readonly IEventsRepository eventsRepository;

        public DeleteEventHandler(IEventsRepository autoTestContext)
        {
            eventsRepository = autoTestContext;
        }

        async Task IRequestHandler<DeleteEvent>.Handle(DeleteEvent request, CancellationToken cancellationToken)
        {
            var found = await this.eventsRepository.GetById(request.EventId, cancellationToken);

            await this.eventsRepository.Delete(found!, cancellationToken);
        }
    }
}
