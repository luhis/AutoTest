using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AutoTest.Web.Hubs
{
    public class ResultsHub : Hub
    {
        public Task ListenToEvent(ulong eventId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, eventId.ToString());
        }
    }
}
