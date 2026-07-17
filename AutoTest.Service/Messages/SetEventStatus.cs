using AutoTest.Domain.Enums;
using Mediator;

namespace AutoTest.Service.Messages;

public record SetEventStatus(ulong EventId, EventStatus Status) : IRequest;
