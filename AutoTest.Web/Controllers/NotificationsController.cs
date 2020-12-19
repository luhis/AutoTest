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
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator mediator;

        public NotificationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(policy: Policies.ClubAdmin)]
        [HttpPost]
        public Task Add(ulong clubId, ulong eventId, Notification notification)
        {
            return mediator.Send(new AddNotification(notification));
        }

        [HttpGet("{eventId}")]
        public Task<IEnumerable<Notification>> GetAll(ulong eventId)
        {
            return mediator.Send(new GetNotifications(eventId));
        }
    }
}
