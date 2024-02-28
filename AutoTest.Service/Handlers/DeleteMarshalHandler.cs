using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class DeleteMarshalHandler(IMarshalsRepository marshalsRepository, IAuthorisationNotifier signalRNotifier) : IRequestHandler<DeleteMarshal>
    {
        async Task IRequestHandler<DeleteMarshal>.Handle(DeleteMarshal request, CancellationToken cancellationToken)
        {
            var found = await marshalsRepository.GetById(request.EventId, request.MarshalId, cancellationToken);
            if (found != null)
            {
                await marshalsRepository.Remove(found, cancellationToken);
                await signalRNotifier.RemoveEventMarshal(request.MarshalId, new[] { found.Email }, cancellationToken);
            }
        }
    }
}
