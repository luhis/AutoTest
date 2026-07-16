using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetEventHandler(IEventsRepository eventsRepository) : IRequestHandler<GetEvent, Event?>
{
    public async ValueTask<Event?> Handle(GetEvent request, CancellationToken cancellationToken)
    {
        return await eventsRepository.GetById(request.EventId, cancellationToken);
    }
}
