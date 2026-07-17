using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetProfile(string EmailAddress) : IRequest<Profile>;
