using System.Collections.Generic;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetEditableEntrants : IRequest<IEnumerable<ulong>>
{
    public string EmailAddress { get; }

    public GetEditableEntrants(string emailAddress)
    {
        EmailAddress = emailAddress;
    }
}
