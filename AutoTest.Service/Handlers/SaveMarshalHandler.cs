using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using Mediator;
using OneOf;
using OneOf.Types;

namespace AutoTest.Service.Handlers;

public sealed class SaveMarshalHandler(IMarshalsRepository marshalRepository, IEventsRepository eventsRepository, IAuthorisationNotifier signalRNotifier) : IRequestHandler<SaveMarshal, OneOf<Marshal, Error<string>>>
{
    public async ValueTask<OneOf<Marshal, Error<string>>> Handle(SaveMarshal request, CancellationToken cancellationToken)
    {
        var @event = await eventsRepository.GetById(request.Marshal.EventId, cancellationToken)
            ?? throw new InvalidOperationException($"Event with id {request.Marshal.EventId} not found");
        if (@event.EventStatus == Domain.Enums.EventStatus.Cancelled)
        {
            return new Error<string>("Event is cancelled");
        }

        var existing = await marshalRepository.GetById(request.Marshal.EventId, request.Marshal.MarshalId, cancellationToken);

        await marshalRepository.Upsert(request.Marshal, cancellationToken);
        if (existing is null || !existing.Email.Equals(request.Marshal.Email, System.StringComparison.OrdinalIgnoreCase))
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
