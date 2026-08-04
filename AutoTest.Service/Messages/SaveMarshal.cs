using AutoTest.Domain.StorageModels;
using Mediator;
using OneOf;
using OneOf.Types;

namespace AutoTest.Service.Messages;

public record SaveMarshal(Marshal Marshal) : IRequest<OneOf<Marshal, Error<string>>>;
