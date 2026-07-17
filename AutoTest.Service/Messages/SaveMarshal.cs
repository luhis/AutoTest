using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record SaveMarshal(Marshal Marshal) : IRequest<Marshal>;
