using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetEntrants(ulong EventId) : IRequest<IEnumerable<Entrant>>;
