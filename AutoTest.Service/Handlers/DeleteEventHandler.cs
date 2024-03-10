using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class DeleteEventHandler(IEventsRepository eventsRepository) : IRequestHandler<DeleteEvent>
    {
        async Task IRequestHandler<DeleteEvent>.Handle(DeleteEvent request, CancellationToken cancellationToken)
        {
            var found = await eventsRepository.GetById(request.EventId, cancellationToken);

            await eventsRepository.Delete(found!, cancellationToken);
        }
    }
}
