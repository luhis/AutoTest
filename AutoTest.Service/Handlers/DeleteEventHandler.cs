using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class DeleteEventHandler(IEventsRepository autoTestContext) : IRequestHandler<DeleteEvent>
    {
        async Task IRequestHandler<DeleteEvent>.Handle(DeleteEvent request, CancellationToken cancellationToken)
        {
            var found = await autoTestContext.GetById(request.EventId, cancellationToken);

            await autoTestContext.Delete(found!, cancellationToken);
        }
    }
}
