using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization.Tooling;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTest.Web.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AccessController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<AccessModel> GetAccessAsync()
    {
        var identity = User.Identity;
        var isAuthenticated = identity is { IsAuthenticated: true };
        var email = User.GetEmailAddress();
        return await mediator.Send(new GetAccess(email, isAuthenticated));
    }
}
