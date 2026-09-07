using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace AutoTest.Web.Hubs;

public class EventHub : Hub
{
    public static string GetEventKey(ulong eventId) => $"eventId:{eventId}";

    private ulong? GetEventId()
    {
        var routeValues = Context.GetHttpContext()?.Request.RouteValues;
        if (routeValues != null && routeValues.TryGetValue("eventId", out var value) && value != null &&
            ulong.TryParse(value.ToString(), out var id))
        {
            return id;
        }
        return null;
    }

    public override async Task OnConnectedAsync()
    {
        var id = GetEventId();
        if (id != null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetEventKey(id.Value));
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var id = GetEventId();
        if (id != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetEventKey(id.Value));
        }
        await base.OnDisconnectedAsync(exception);
    }
}
