using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetMarshals(ulong EventId) : IRequest<IEnumerable<Marshal>>;
