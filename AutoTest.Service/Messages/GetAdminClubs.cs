using System.Collections.Generic;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetAdminClubs(string EmailAddress) : IRequest<IEnumerable<ulong>>;
