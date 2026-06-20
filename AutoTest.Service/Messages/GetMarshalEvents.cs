using System.Collections.Generic;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetMarshalEvents : IRequest<IEnumerable<ulong>>
{
    public string EmailAddress { get; }

    public GetMarshalEvents(string emailAddress)
    {
        EmailAddress = emailAddress;
    }
}
