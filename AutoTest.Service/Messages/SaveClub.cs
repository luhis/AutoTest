using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record SaveClub(Club Club) : IRequest<ulong>;
