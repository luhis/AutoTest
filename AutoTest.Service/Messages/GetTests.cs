using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetTests(ulong EventId) : IRequest<IEnumerable<Course>>;
