using System;
using System.Threading.Tasks;
using AutoTest.Web.Authorization.Tooling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AutoTest.Web.Hubs;

[Authorize]
public class AuthorisationHub : Hub
{
    public static string GetEmailKey(string email) => $"email:{email}";
    public override async Task OnConnectedAsync()
    {
        var email = Context.User!.GetEmailAddress();

        if (!string.IsNullOrEmpty(email))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetEmailKey(email));
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var email = Context.User!.GetEmailAddress();

        if (!string.IsNullOrEmpty(email))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetEmailKey(email));
        }
        await base.OnDisconnectedAsync(exception);
    }
}
