using System;
using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record UpdateTestRun(
    ulong TestRunId,
    ulong EventId,
    int Ordinal,
    int TimeInMS,
    ulong EntrantId,
    DateTime Created,
    ulong MarshalId,
    IEnumerable<Penalty> Penalties) : IRequest;
