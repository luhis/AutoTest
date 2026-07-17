using AutoTest.Domain.Enums;
using Mediator;

namespace AutoTest.Service.Messages;

public record SetEntrantStatus(ulong EventId, ulong EntrantId, EntrantStatus Status) : IRequest;
