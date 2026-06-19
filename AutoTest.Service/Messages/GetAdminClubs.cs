using System.Collections.Generic;
using MediatR;

namespace AutoTest.Service.Messages;

public class GetAdminClubs : IRequest<IEnumerable<ulong>>
{
    public string EmailAddress { get; }

    public GetAdminClubs(string emailAddress)
    {
        EmailAddress = emailAddress;
    }
}
