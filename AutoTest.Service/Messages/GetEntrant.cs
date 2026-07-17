using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetEntrant(ulong EventId, ulong EntrantId) : IRequest<Entrant?>;
