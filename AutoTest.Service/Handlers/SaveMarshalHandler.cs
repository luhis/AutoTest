using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers;

public class SaveMarshalHandler(IMarshalsRepository marshalRepository, IAuthorisationNotifier signalRNotifier) : IRequestHandler<SaveMarshal, Marshal>
{
    async Task<Marshal> IRequestHandler<SaveMarshal, Marshal>.Handle(SaveMarshal request, CancellationToken cancellationToken)
    {
        var existing = await marshalRepository.GetById(request.Marshal.EventId, request.Marshal.MarshalId, cancellationToken);

        await marshalRepository.Upsert(request.Marshal, cancellationToken);
        if (existing is null || !existing!.Email.Equals(request.Marshal.Email, System.StringComparison.OrdinalIgnoreCase))
        {
            await signalRNotifier.NewEventMarshal(request.Marshal.EventId, [request.Marshal.Email], cancellationToken);
            if (existing is not null)
            {
                await signalRNotifier.RemoveEventMarshal(request.Marshal.EventId, [existing.Email], cancellationToken);
            }
        }
        await signalRNotifier.AddEditableMarshal(request.Marshal.MarshalId, [request.Marshal.Email], cancellationToken);
        return request.Marshal;
    }
}
