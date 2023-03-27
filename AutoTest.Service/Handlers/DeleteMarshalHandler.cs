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
        private readonly IAuthorisationNotifier authorisationNotifier;
        private readonly IMarshalsRepository _marshalsRepository;

        public DeleteMarshalHandler(IMarshalsRepository marshalsRepository, IAuthorisationNotifier signalRNotifier)
        {
            _marshalsRepository = marshalsRepository;
            this.authorisationNotifier = signalRNotifier;
        }

        async Task IRequestHandler<DeleteMarshal>.Handle(DeleteMarshal request, CancellationToken cancellationToken)
        {
            var found = await _marshalsRepository.GetById(request.EventId, request.MarshalId, cancellationToken);
            if (found != null)
            {
                await _marshalsRepository.Remove(found, cancellationToken);
                await authorisationNotifier.RemoveEventMarshal(request.MarshalId, new[] { found.Email }, cancellationToken);
            }
        }
    }
}
