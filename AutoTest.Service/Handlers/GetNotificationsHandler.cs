using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetNotificationsHandler : IRequestHandler<GetNotifications, IEnumerable<Notification>>
    {
        private readonly INotificationsRepository notificationsRepository;

        public GetNotificationsHandler(INotificationsRepository notificationsRepository)
        {
            this.notificationsRepository = notificationsRepository;
        }

        Task<IEnumerable<Notification>> IRequestHandler<GetNotifications, IEnumerable<Notification>>.Handle(GetNotifications request, CancellationToken cancellationToken)
        {
            return notificationsRepository.GetNotifications(request.EventId, cancellationToken);
        }
    }
}
