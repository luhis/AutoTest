namespace AutoTest.Web.Controllers
{
    using AutoTest.Web.Models;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AccessController : ControllerBase
    {

        private readonly IMediator mediator;

        public AccessController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public AccessModel GetAccess()
        {
            return new AccessModel(true);
        }
    }
}