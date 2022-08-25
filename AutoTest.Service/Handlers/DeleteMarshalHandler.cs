using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class DeleteMarshalHandler : IRequestHandler<DeleteMarshal>
    {
        private readonly ISignalRNotifier signalRNotifier;
        private readonly IMarshalsRepository _marshalsRepository
            ;

        public DeleteMarshalHandler(IMarshalsRepository marshalsRepository, ISignalRNotifier signalRNotifier)
        {
            _marshalsRepository = marshalsRepository;
            this.signalRNotifier = signalRNotifier;
        }

        async Task<Unit> IRequestHandler<DeleteMarshal, Unit>.Handle(DeleteMarshal request, CancellationToken cancellationToken)
        {
            var found = await _marshalsRepository.GetById(request.EventId, request.MarshalId, cancellationToken);
            if (found != null)
            {
                await _marshalsRepository.Remove(found, cancellationToken);
                await signalRNotifier.RemoveEventMarshal(request.MarshalId, new[] { found.Email }, cancellationToken);
            }
            return Unit.Value;
        }
    }
}
