using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class DeleteEventHandler(IEventsRepository eventsRepository) : IRequestHandler<DeleteEvent>
{
    public async ValueTask<Unit> Handle(DeleteEvent request, CancellationToken cancellationToken)
    {
        var found = await eventsRepository.GetById(request.EventId, cancellationToken) ?? throw new InvalidOperationException("Event not found");

        await eventsRepository.Delete(found, cancellationToken);
        return Unit.Value;
    }
}
