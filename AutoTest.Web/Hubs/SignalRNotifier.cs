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
        private readonly IHubContext<ResultsHub> hub;
        private readonly IMediator mediator;

        public SignalRNotifier(IHubContext<ResultsHub> hub, IMediator mediator)
        {
            this.hub = hub;
            this.mediator = mediator;
        }

        private IClientProxy GetEventGroup(ulong eventId) => this.hub.Clients.Group(eventId.ToString());

        async Task ISignalRNotifier.NewTestRun(TestRun testRun, CancellationToken cancellationToken)
        {
            var results = await mediator.Send(new GetResults(testRun.EventId), cancellationToken);
            var group = GetEventGroup(testRun.EventId);
            await group.SendAsync("NewResults", results, cancellationToken);
            await group.SendAsync("NewTestRun", testRun, cancellationToken);
        }

        Task ISignalRNotifier.NewNotification(Notification notification, CancellationToken cancellationToken)
        {
            return GetEventGroup(notification.EventId).SendAsync("NewNotification", notification, cancellationToken);
        }
    }
}
