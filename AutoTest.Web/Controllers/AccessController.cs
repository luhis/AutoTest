using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization.Tooling;
using AutoTest.Web.Models.Display;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AutoTest.Web.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AccessController(IConfiguration configuration, IMediator mediator) : ControllerBase
{
    private HashSet<string> RootAdminEmails { get; } = new HashSet<string>(configuration.GetSection("RootAdminIds").Get<IEnumerable<string>>() ?? [], StringComparer.OrdinalIgnoreCase);

    [HttpGet]
    public async Task<AccessModel> GetAccessAsync()
    {
        var identity = User.Identity;
        var isAuthenticated = identity is { IsAuthenticated: true };
        var email = User.GetEmailAddress();
        var adminClubsTask = mediator.Send(new GetAdminClubs(email)).AsTask();
        var marshalEventsTask = mediator.Send(new GetMarshalEvents(email)).AsTask();
        var editableEntrantsTask = mediator.Send(new GetEditableEntrants(email)).AsTask();
        var editableMarshalsTask = mediator.Send(new GetEditableMarshals(email)).AsTask();
        await Task.WhenAll(adminClubsTask, marshalEventsTask, editableEntrantsTask, editableMarshalsTask);
        return new AccessModel(RootAdminEmails.Contains(email), isAuthenticated,
            await adminClubsTask, await marshalEventsTask,
            await editableEntrantsTask, await editableMarshalsTask);
    }
}
