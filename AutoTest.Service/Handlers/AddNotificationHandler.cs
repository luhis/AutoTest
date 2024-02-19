using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class AddNotificationHandler : IRequestHandler<AddNotification>
    {
        private readonly INotificationsRepository notificationsRepository;
        private readonly IEventNotifier signalRNotifier;

        public AddNotificationHandler(INotificationsRepository notificationsRepository, IEventNotifier signalRNotifier)
        {
            this.notificationsRepository = notificationsRepository;
            this.signalRNotifier = signalRNotifier;
        }

        public async Task Handle(AddNotification request, CancellationToken cancellationToken)
        {
            await notificationsRepository.AddNotification(request.Notification, cancellationToken);
            await signalRNotifier.NewNotification(request.Notification, cancellationToken);
        }
    }
}
