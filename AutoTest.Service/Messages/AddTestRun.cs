using System;
using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using Mediator;
using OneOf;
using OneOf.Types;

namespace AutoTest.Service.Messages;

public record AddTestRun(
    ulong TestRunId,
    ulong EventId,
    int Ordinal,
    int TimeInMS,
    ulong EntrantId,
    DateTime Created,
    string EmailAddress,
    IEnumerable<Penalty> Penalties) : IRequest<OneOf<Success, Error<string>>>;
