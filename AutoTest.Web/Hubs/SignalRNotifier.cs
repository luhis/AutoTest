using System.Threading;
using System.Threading.Tasks;
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

        async Task ISignalRNotifier.NewTestRun(ulong eventId, CancellationToken cancellationToken)
        {
            var results = await mediator.Send(new GetResults(eventId), cancellationToken);
            await this.hub.Clients.Group(eventId.ToString()).SendAsync("newTestRun", results, cancellationToken);
        }
    }
}
