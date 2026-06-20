using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetNotificationsHandler(INotificationsRepository notificationsRepository) : IRequestHandler<GetNotifications, IEnumerable<Notification>>
{
    public async ValueTask<IEnumerable<Notification>> Handle(GetNotifications request, CancellationToken cancellationToken)
    {
        return await notificationsRepository.GetNotifications(request.EventId, cancellationToken);
    }
}
