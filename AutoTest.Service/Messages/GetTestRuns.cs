using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetTestRuns(ulong EventId, int Ordinal) : IRequest<IEnumerable<TestRun>>;
