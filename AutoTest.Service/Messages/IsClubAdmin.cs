using Mediator;

namespace AutoTest.Service.Messages;

public class IsClubAdmin : IRequest<bool>
{
    public IsClubAdmin(ulong eventId, string emailAddress)
    {
        EventId = eventId;
        EmailAddress = emailAddress;
    }

    public ulong EventId { get; }
    public string EmailAddress { get; }
}
