using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class AddNotificationHandler(INotificationsRepository notificationsRepository, IEventNotifier signalRNotifier) : IRequestHandler<AddNotification>
    {
        public async Task Handle(AddNotification request, CancellationToken cancellationToken)
        {
            await notificationsRepository.AddNotification(request.Notification, cancellationToken);
            await signalRNotifier.NewNotification(request.Notification, cancellationToken);
        }
    }
}
