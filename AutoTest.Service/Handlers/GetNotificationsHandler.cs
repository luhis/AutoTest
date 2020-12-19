using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetNotificationsHandler : IRequestHandler<GetNotifications, IEnumerable<Notification>>
    {
        private readonly AutoTestContext autoTestContext;

        public GetNotificationsHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        Task<IEnumerable<Notification>> IRequestHandler<GetNotifications, IEnumerable<Notification>>.Handle(GetNotifications request, CancellationToken cancellationToken)
        {
            return autoTestContext.Notifications.Where(a => a.EventId == request.EventId).ToEnumerableAsync(cancellationToken);
        }
    }
}
