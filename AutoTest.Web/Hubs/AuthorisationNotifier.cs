using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Service.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AutoTest.Web.Hubs;

public class AuthorisationNotifier(IHubContext<AuthorisationHub> authorisationHub) : IAuthorisationNotifier
{
    Task IAuthorisationNotifier.AddEditableEntrant(ulong eventId, IEnumerable<string> newEmails, CancellationToken cancellationToken)
        => SendToEmails(nameof(IAuthorisationNotifier.AddEditableEntrant), eventId, newEmails, cancellationToken);

    Task IAuthorisationNotifier.AddEditableMarshal(ulong eventId, IEnumerable<string> newEmails, CancellationToken cancellationToken)
        => SendToEmails(nameof(IAuthorisationNotifier.AddEditableMarshal), eventId, newEmails, cancellationToken);

    Task IAuthorisationNotifier.NewClubAdmin(ulong clubId, IEnumerable<string> newEmails, CancellationToken cancellationToken)
        => SendToEmails(nameof(IAuthorisationNotifier.NewClubAdmin), clubId, newEmails, cancellationToken);

    Task IAuthorisationNotifier.NewEventMarshal(ulong eventId, IEnumerable<string> newEmails, CancellationToken cancellationToken)
        => SendToEmails(nameof(IAuthorisationNotifier.NewEventMarshal), eventId, newEmails, cancellationToken);

    Task IAuthorisationNotifier.RemoveClubAdmin(ulong clubId, IEnumerable<string> newEmails, CancellationToken cancellationToken)
        => SendToEmails(nameof(IAuthorisationNotifier.RemoveClubAdmin), clubId, newEmails, cancellationToken);

    Task IAuthorisationNotifier.RemoveEventMarshal(ulong eventId, IEnumerable<string> newEmails, CancellationToken cancellationToken)
        => SendToEmails(nameof(IAuthorisationNotifier.RemoveEventMarshal), eventId, newEmails, cancellationToken);

    private Task SendToEmails(string methodName, ulong id, IEnumerable<string> emails, CancellationToken cancellationToken)
    {
        var groups = emails.Select(e => authorisationHub.Clients.Group(AuthorisationHub.GetEmailKey(e)));
        return Task.WhenAll(groups.Select(a => a.SendAsync(methodName, id, cancellationToken)));
    }
}
