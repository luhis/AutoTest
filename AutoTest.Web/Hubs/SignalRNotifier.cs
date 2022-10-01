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
        private readonly IMediator mediator;

        public SignalRNotifier(IHubContext<ResultsHub> resultsHub, IMediator mediator)
        {
            this.resultsHub = resultsHub;
            this.mediator = mediator;
        }

        private IClientProxy GetEventGroup(ulong eventId) => this.resultsHub.Clients.Group(ResultsHub.GetEventKey(eventId));

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
    }
}
