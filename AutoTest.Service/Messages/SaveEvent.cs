using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record SaveEvent(Event Event, string Maps, string Regulations) : IRequest<ulong>;
