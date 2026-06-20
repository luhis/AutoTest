using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class DeleteMarshalHandler(IMarshalsRepository marshalsRepository, IAuthorisationNotifier signalRNotifier) : IRequestHandler<DeleteMarshal>
{
    public async ValueTask<Unit> Handle(DeleteMarshal request, CancellationToken cancellationToken)
    {
        var found = await marshalsRepository.GetById(request.EventId, request.MarshalId, cancellationToken);
        if (found is not null)
        {
            await marshalsRepository.Remove(found, cancellationToken);
            await signalRNotifier.RemoveEventMarshal(request.MarshalId, [found.Email], cancellationToken);
        }
        return Unit.Value;
    }
}
