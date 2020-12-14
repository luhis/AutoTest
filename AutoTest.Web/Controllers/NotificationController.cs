using System.Collections.Generic;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTest.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator mediator;

        public NotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(policy: Policies.ClubAdmin)]
        [HttpPost]
        public Task Add(ulong clubId, ulong eventId)
        {
            return mediator.Send(new AddNotification(eventId));
        }

        [HttpGet]
        public Task<IEnumerable<Notification>> GetAll(ulong eventId)
        {
            return mediator.Send(new GetNotifications(eventId));
        }
    }
}
