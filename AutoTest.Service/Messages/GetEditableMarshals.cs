using System.Collections.Generic;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetEditableMarshals(string EmailAddress) : IRequest<IEnumerable<ulong>>;
