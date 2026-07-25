using System.Collections.Generic;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetMarshalEvents(string EmailAddress) : IRequest<IEnumerable<ulong>>;
