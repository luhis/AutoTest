using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Service.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AutoTest.Web.Hubs
{
    public class AuthorisationNotifier : IAuthorisationNotifier
    {
        private readonly IHubContext<AuthorisationHub> authorisationHub;

        public AuthorisationNotifier(IHubContext<AuthorisationHub> authorisationHub)
        {
            this.authorisationHub = authorisationHub;
        }

        private IClientProxy GetEmailGroup(string email) => this.authorisationHub.Clients.Group(AuthorisationHub.GetEmailKey(email));

        Task IAuthorisationNotifier.NewClubAdmin(ulong clubId, IEnumerable<string> newEmails)
        {
            var groups = newEmails.Select(e => GetEmailGroup(e));
            return Task.WhenAll(groups.Select(a => a.SendAsync(nameof(IAuthorisationNotifier.NewClubAdmin), clubId)));
        }

        Task IAuthorisationNotifier.NewEventMarshal(ulong eventId, IEnumerable<string> newEmails, CancellationToken cancellationToken)
        {
            var groups = newEmails.Select(e => GetEmailGroup(e));
            return Task.WhenAll(groups.Select(a => a.SendAsync(nameof(IAuthorisationNotifier.NewEventMarshal), eventId, cancellationToken)));
        }

        Task IAuthorisationNotifier.RemoveClubAdmin(ulong clubId, IEnumerable<string> newEmails)
        {
            var groups = newEmails.Select(e => GetEmailGroup(e));
            return Task.WhenAll(groups.Select(a => a.SendAsync(nameof(IAuthorisationNotifier.RemoveClubAdmin), clubId)));
        }

        Task IAuthorisationNotifier.RemoveEventMarshal(ulong eventId, IEnumerable<string> newEmails, CancellationToken cancellationToken)
        {
            var groups = newEmails.Select(e => GetEmailGroup(e));
            return Task.WhenAll(groups.Select(a => a.SendAsync(nameof(IAuthorisationNotifier.RemoveEventMarshal), eventId, cancellationToken)));
        }
    }
}
