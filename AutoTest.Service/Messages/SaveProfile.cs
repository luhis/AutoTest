using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record SaveProfile(Profile Profile) : IRequest<Profile>;
