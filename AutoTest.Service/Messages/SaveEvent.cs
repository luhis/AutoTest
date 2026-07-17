using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record SaveEvent(Event Event) : IRequest<ulong>;
