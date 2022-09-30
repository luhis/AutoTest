using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace AutoTest.Web.Hubs
{
    public class ResultsHub : Hub
    {
        public static string GetEventKey(ulong eventId) => $"eventId:{eventId}";

        public Task ListenToEvent(ulong eventId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, GetEventKey(eventId));
        }
        public Task LeaveEvent(ulong eventId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, GetEventKey(eventId));
        }
    }
}
