using AutoTest.Service.Models;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetAwards(ulong EventId) : IRequest<Awards>;
