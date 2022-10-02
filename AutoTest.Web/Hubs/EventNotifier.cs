using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace AutoTest.Web.Hubs
{
    public class EventNotifier : IEventNotifier
    {
        private readonly IHubContext<EventHub> eventHub;
        private readonly IMediator mediator;

        public EventNotifier(IHubContext<EventHub> eventHub, IMediator mediator)
        {
            this.eventHub = eventHub;
            this.mediator = mediator;
        }

        private IClientProxy GetEventGroup(ulong eventId) => this.eventHub.Clients.Group(EventHub.GetEventKey(eventId));

        async Task IEventNotifier.NewTestRun(TestRun testRun, CancellationToken cancellationToken)
        {
            var results = await mediator.Send(new GetResults(testRun.EventId), cancellationToken);
            var group = GetEventGroup(testRun.EventId);
            await group.SendAsync("NewResults", results, cancellationToken);
            await group.SendAsync("NewTestRun", testRun, cancellationToken);
        }

        Task IEventNotifier.NewNotification(Notification notification, CancellationToken cancellationToken)
        {
            return GetEventGroup(notification.EventId).SendAsync(nameof(IEventNotifier.NewNotification), notification, cancellationToken);
        }
    }
}
