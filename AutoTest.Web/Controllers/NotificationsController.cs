using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization;
using AutoTest.Web.Authorization.Tooling;
using AutoTest.Web.Mapping;
using AutoTest.Web.Models.Save;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTest.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/{eventId}")]
    public class NotificationsController(IMediator mediator) : ControllerBase
    {
        [Authorize(policy: Policies.ClubAdmin)]
        [HttpPut("{notificationId}")]
        public Task Add(ulong notificationId, ulong eventId, NotificationSaveModel notification, CancellationToken cancellationToken)
        {
            var email = this.User.GetEmailAddress();
            return mediator.Send(new AddNotification(MapClub.Map(notificationId, eventId, email, notification)), cancellationToken);
        }

        [HttpGet]
        public Task<IEnumerable<Notification>> GetAll(ulong eventId, CancellationToken cancellationToken)
        {
            return mediator.Send(new GetNotifications(eventId), cancellationToken);
        }
    }
}
