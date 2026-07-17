using System.Collections.Generic;
using AutoTest.Service.Models;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetResults(ulong EventId) : IRequest<IEnumerable<Result>>;
