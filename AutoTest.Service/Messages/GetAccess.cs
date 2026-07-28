using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetAccess(string EmailAddress, bool IsAuthenticated) : IRequest<AccessModel>;
