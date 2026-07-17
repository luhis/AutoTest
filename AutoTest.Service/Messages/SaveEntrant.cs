using AutoTest.Domain.StorageModels;
using Mediator;
using OneOf;
using OneOf.Types;

namespace AutoTest.Service.Messages;

public record SaveEntrant(Entrant Entrant) : IRequest<OneOf<Entrant, Error<string>>>;
