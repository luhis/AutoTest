using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetMarshal(ulong EventId, ulong MarshalId) : IRequest<Marshal?>;
