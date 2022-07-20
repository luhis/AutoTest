using System.Threading.Tasks;
using AutoTest.Web.Authorization.Tooling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AutoTest.Web.Hubs
{
    [Authorize]
    public class AuthorisationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var email = Context.User!.GetEmailAddress();

            if (email != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, email);
            }

            await base.OnConnectedAsync();
        }
    }
}
