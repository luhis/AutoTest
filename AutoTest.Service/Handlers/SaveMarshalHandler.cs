using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SaveMarshalHandler : IRequestHandler<SaveMarshal, Marshal>
    {
        private readonly IMarshalsRepository _marshalRepository;
        private readonly IAuthorisationNotifier signalRNotifier;

        public SaveMarshalHandler(IMarshalsRepository marshalRepository, IAuthorisationNotifier signalRNotifier)
        {
            this._marshalRepository = marshalRepository;
            this.signalRNotifier = signalRNotifier;
        }

        async Task<Marshal> IRequestHandler<SaveMarshal, Marshal>.Handle(SaveMarshal request, CancellationToken cancellationToken)
        {
            var existing = await _marshalRepository.GetById(request.Marshal.EventId, request.Marshal.MarshalId, cancellationToken);

            await _marshalRepository.Upsert(request.Marshal, cancellationToken);
            if (existing == null || !existing!.Email.Equals(request.Marshal.Email, System.StringComparison.InvariantCultureIgnoreCase))
            {
                await signalRNotifier.NewEventMarshal(request.Marshal.EventId, new[] { request.Marshal.Email }, cancellationToken);
                if (existing != null)
                {
                    await signalRNotifier.RemoveEventMarshal(request.Marshal.EventId, new[] { existing.Email }, cancellationToken);
                }
            }
            return request.Marshal;
        }
    }
}
