using System.Threading.Tasks;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization.Tooling;
using AutoTest.Web.Models.Display;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTest.Web.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AccessController(RootAdminEmails rootAdminEmails, IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<AccessModel> GetAccessAsync()
    {
        var identity = User.Identity;
        var isAuthenticated = identity is { IsAuthenticated: true };
        var email = User.GetEmailAddress();
        var adminClubs = await mediator.Send(new GetAdminClubs(email));
        var marshalEvents = await mediator.Send(new GetMarshalEvents(email));
        var editableEntrants = await mediator.Send(new GetEditableEntrants(email));
        var editableMarshals = await mediator.Send(new GetEditableMarshals(email));
        return new AccessModel(rootAdminEmails.Contains(email), isAuthenticated,
            adminClubs, marshalEvents,
            editableEntrants, editableMarshals);
    }
}
