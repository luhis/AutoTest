using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetNotifications(ulong EventId) : IRequest<IEnumerable<Notification>>;
