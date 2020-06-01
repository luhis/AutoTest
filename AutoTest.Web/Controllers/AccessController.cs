namespace AutoTest.Web.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AccessController : ControllerBase
    {

        private readonly IMediator mediator;
    }
}