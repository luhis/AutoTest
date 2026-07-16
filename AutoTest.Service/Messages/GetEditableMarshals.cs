using System.Collections.Generic;
using Mediator;

namespace AutoTest.Service.Messages;

public record GetEditableMarshals : IRequest<IEnumerable<ulong>>
{
    public string EmailAddress { get; }

    public GetEditableMarshals(string emailAddress)
    {
        EmailAddress = emailAddress;
    }
}
