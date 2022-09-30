using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace AutoTest.Web.Hubs
{
    public class SignalRNotifier : ISignalRNotifier
    {
        private readonly IHubContext<ResultsHub> resultsHub;
        private readonly IHubContext<AuthorisationHub> authorisationHub;
        private readonly IMediator mediator;

        public SignalRNotifier(IHubContext<ResultsHub> resultsHub, IMediator mediator, IHubContext<AuthorisationHub> authorisationHub)
        {
            this.resultsHub = resultsHub;
            this.mediator = mediator;
            this.authorisationHub = authorisationHub;
        }

        private IClientProxy GetEventGroup(ulong eventId) => this.resultsHub.Clients.Group(ResultsHub.GetEventKey(eventId));
        private IClientProxy GetEmailGroup(string email) => this.authorisationHub.Clients.Group(AuthorisationHub.GetEmailKey(email));


        async Task ISignalRNotifier.NewTestRun(TestRun testRun, CancellationToken cancellationToken)
        {
            var results = await mediator.Send(new GetResults(testRun.EventId), cancellationToken);
            var group = GetEventGroup(testRun.EventId);
            await group.SendAsync("NewResults", results, cancellationToken);
            await group.SendAsync("NewTestRun", testRun, cancellationToken);
        }

        Task ISignalRNotifier.NewNotification(Notification notification, CancellationToken cancellationToken)
        {
            return GetEventGroup(notification.EventId).SendAsync(nameof(ISignalRNotifier.NewNotification), notification, cancellationToken);
        }

        Task ISignalRNotifier.NewClubAdmin(ulong clubId, IEnumerable<string> newEmails)
        {
            var groups = newEmails.Select(e => GetEmailGroup(e));
            return Task.WhenAll(groups.Select(a => a.SendAsync(nameof(ISignalRNotifier.NewClubAdmin), clubId)));
        }

        Task ISignalRNotifier.NewEventMarshal(ulong eventId, IEnumerable<string> newEmails, CancellationToken cancellationToken)
        {
            var groups = newEmails.Select(e => GetEmailGroup(e));
            return Task.WhenAll(groups.Select(a => a.SendAsync(nameof(ISignalRNotifier.NewEventMarshal), eventId, cancellationToken)));
        }

        Task ISignalRNotifier.RemoveClubAdmin(ulong clubId, IEnumerable<string> newEmails)
        {
            var groups = newEmails.Select(e => GetEmailGroup(e));
            return Task.WhenAll(groups.Select(a => a.SendAsync(nameof(ISignalRNotifier.RemoveClubAdmin), clubId)));
        }

        Task ISignalRNotifier.RemoveEventMarshal(ulong eventId, IEnumerable<string> newEmails, CancellationToken cancellationToken)
        {
            var groups = newEmails.Select(e => GetEmailGroup(e));
            return Task.WhenAll(groups.Select(a => a.SendAsync(nameof(ISignalRNotifier.RemoveEventMarshal), eventId, cancellationToken)));
        }
    }
}
