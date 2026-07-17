using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization.Tooling;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

namespace AutoTest.Web.Authorization;

public static class AuthTools
{
    public static ulong GetEventId(RouteData routeData)
    {
        if (routeData.Values.TryGetValue(RouteParams.EventId, out var eventIdString) && eventIdString is not null)
        {
            return ulong.Parse((string)eventIdString, CultureInfo.InvariantCulture);
        }
        throw new ArgumentException("Don't know how to get EventId from this request");
    }

    public static async Task<string?> GetExistingEmail(RouteData routeData, IMediator mediator)
    {
        if (routeData.Values.TryGetValue(RouteParams.EventId, out var eventIdString) && eventIdString is not null)
        {
            var eventId = ulong.Parse((string)eventIdString, CultureInfo.InvariantCulture);
            if (routeData.Values.TryGetValue(RouteParams.EntrantId, out var entrantIdString) && entrantIdString is not null)
            {
                var entrantId = ulong.Parse((string)entrantIdString, CultureInfo.InvariantCulture);
                var entrant = await mediator.Send(new GetEntrant(eventId, entrantId));
                return entrant?.Email;
            }
            if (routeData.Values.TryGetValue(RouteParams.MarshalId, out var marshalIdString) && marshalIdString is not null)
            {
                var marshalId = ulong.Parse((string)marshalIdString, CultureInfo.InvariantCulture);
                var existing = await mediator.Send(new GetMarshal(eventId, marshalId));
                return existing?.Email;
            }
        }
        throw new ArgumentException("Don't know how to get Email from this request");
    }

    public enum ClubAdminResult
    {
        NewEvent,
        ClubNotFound,
        IsAdmin,
        NotAdmin
    }

    public static async Task<ClubAdminResult> CheckClubAdmin(ulong eventId, string email, IMediator mediator)
    {
        var @event = await mediator.Send(new GetEvent(eventId), CancellationToken.None);
        if (@event is null)
        {
            return ClubAdminResult.NewEvent;
        }

        var club = await mediator.Send(new GetClub(@event.ClubId));
        if (club is null)
        {
            return ClubAdminResult.ClubNotFound;
        }

        var emails = club.AdminEmails.Select(b => b.Email).ToHashSet(StringComparer.OrdinalIgnoreCase);
        return emails.Contains(email) ? ClubAdminResult.IsAdmin : ClubAdminResult.NotAdmin;
    }

    public static async Task<bool> IsSelf(AuthorizationHandlerContext context, RouteData routeData, IMediator mediator)
    {
        var emailFromRoute = await GetExistingEmail(routeData, mediator);
        var email = context.User.GetEmailAddress();
        return emailFromRoute is not null &&
            emailFromRoute.Equals(email, StringComparison.OrdinalIgnoreCase);
    }
}
