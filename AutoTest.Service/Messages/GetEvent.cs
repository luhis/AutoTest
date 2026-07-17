using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetEvent(ulong EventId) : IRequest<Event?>;
