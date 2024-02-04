using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace AutoTest.Web.Hubs
{
    public class EventNotifier(IHubContext<EventHub> eventHub, IMediator mediator) : IEventNotifier
    {
        private IClientProxy GetEventGroup(ulong eventId) => eventHub.Clients.Group(EventHub.GetEventKey(eventId));

        async Task IEventNotifier.NewTestRun(TestRun testRun, CancellationToken cancellationToken)
        {
            var results = await mediator.Send(new GetResults(testRun.EventId), cancellationToken);
            var group = GetEventGroup(testRun.EventId);
            await group.SendAsync("NewResults", results, cancellationToken);
            await group.SendAsync("NewTestRun", testRun, cancellationToken);
        }

        Task IEventNotifier.NewNotification(Notification notification, CancellationToken cancellationToken) => GetEventGroup(notification.EventId).SendAsync(nameof(IEventNotifier.NewNotification), notification, cancellationToken);

        Task IEventNotifier.EventStatusChanged(ulong eventId, EventStatus newStatus, CancellationToken cancellationToken)
       => GetEventGroup(eventId).SendAsync(nameof(IEventNotifier.EventStatusChanged), eventId, newStatus, cancellationToken);
    }
}
