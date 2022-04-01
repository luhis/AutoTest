using System;
using System.Threading.Tasks;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace AutoTest.Web.Authorization
{
    public static class AuthTools
    {
        public static ulong GetEventId(RouteData routeData)
        {
            if (routeData.Values.TryGetValue("eventId", out var eventIdString) && eventIdString != null)
            {
                return ulong.Parse((string)eventIdString);
            }
            else if (routeData.Values.TryGetValue("entrantId", out var entrantIdString) && entrantIdString != null)
            {
                throw new Exception("i don't think this route is used anymore");
            }
            throw new Exception("Don't know how to get EventId from this request");
        }

        public static async Task<string> GetEmail(RouteData routeData, IMediator mediator)
        {
            if (routeData.Values.TryGetValue("eventId", out var eventIdString) && eventIdString != null)
            {
                var eventId = ulong.Parse((string)eventIdString);
                if (routeData.Values.TryGetValue("entrantId", out var entrantIdString) && entrantIdString != null)
                {
                    var entrantId = ulong.Parse((string)entrantIdString);
                    return (await mediator.Send(new GetEntrant(eventId, entrantId)))!.Email;
                }
                if (routeData.Values.TryGetValue("marshalId", out var marshalIdString) && marshalIdString != null)
                {
                    var marshalId = ulong.Parse((string)marshalIdString);
                    return (await mediator.Send(new GetMarshal(eventId, marshalId))).Email;
                }
            }
            throw new Exception("Don't know how to get Email from this request");
        }
    }
}
