using System.Collections.Generic;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization;
using AutoTest.Web.Authorization.Tooling;
using AutoTest.Web.Mapping;
using AutoTest.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTest.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/{eventId}")]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator mediator;

        public NotificationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(policy: Policies.ClubAdmin)]
        [HttpPut("{notificationId}")]
        public Task Add(ulong notificationId, ulong eventId, NotificationSaveModel notification)
        {
            var email = this.User.GetEmailAddress();
            return mediator.Send(new AddNotification(MapClub.Map(notificationId, eventId, email, notification)));
        }

        [HttpGet]
        public Task<IEnumerable<Notification>> GetAll(ulong eventId)
        {
            return mediator.Send(new GetNotifications(eventId));
        }
    }
}
