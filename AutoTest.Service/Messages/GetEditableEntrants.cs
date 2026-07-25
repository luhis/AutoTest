using System.Collections.Generic;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetEditableEntrants(string EmailAddress) : IRequest<IEnumerable<ulong>>;
