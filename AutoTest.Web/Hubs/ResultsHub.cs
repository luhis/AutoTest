using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace AutoTest.Web.Hubs
{
    public class ResultsHub : Hub
    {
        public Task ListenToEvent(ulong eventId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, eventId.ToString());
        }
        public Task LeaveEvent(ulong eventId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, eventId.ToString());
        }
    }
}
