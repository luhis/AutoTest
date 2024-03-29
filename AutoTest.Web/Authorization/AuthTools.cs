﻿using System;
using System.Globalization;
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
            if (routeData.Values.TryGetValue(RouteParams.EventId, out var eventIdString) && eventIdString != null)
            {
                return ulong.Parse((string)eventIdString, CultureInfo.InvariantCulture);
            }
            else if (routeData.Values.TryGetValue(RouteParams.EntrantId, out var entrantIdString) && entrantIdString != null)
            {
                throw new ArgumentException("i don't think this route is used anymore");
            }
            throw new ArgumentException("Don't know how to get EventId from this request");
        }

        public static async Task<string?> GetExistingEmail(RouteData routeData, IMediator mediator)
        {
            if (routeData.Values.TryGetValue(RouteParams.EventId, out var eventIdString) && eventIdString != null)
            {
                var eventId = ulong.Parse((string)eventIdString, CultureInfo.InvariantCulture);
                if (routeData.Values.TryGetValue(RouteParams.EntrantId, out var entrantIdString) && entrantIdString != null)
                {
                    var entrantId = ulong.Parse((string)entrantIdString, CultureInfo.InvariantCulture);
                    var entrant = await mediator.Send(new GetEntrant(eventId, entrantId));
                    return entrant?.Email;
                }
                if (routeData.Values.TryGetValue(RouteParams.MarshalId, out var marshalIdString) && marshalIdString != null)
                {
                    var marshalId = ulong.Parse((string)marshalIdString, CultureInfo.InvariantCulture);
                    var existing = await mediator.Send(new GetMarshal(eventId, marshalId));
                    return existing?.Email;
                }
            }
            throw new ArgumentException("Don't know how to get Email from this request");
        }
    }
}
