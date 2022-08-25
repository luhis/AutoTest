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
        private readonly ISignalRNotifier signalRNotifier;

        public AddNotificationHandler(INotificationsRepository notificationsRepository, ISignalRNotifier signalRNotifier)
        {
            this.notificationsRepository = notificationsRepository;
            this.signalRNotifier = signalRNotifier;
        }

        async Task<Unit> IRequestHandler<AddNotification, Unit>.Handle(AddNotification request, CancellationToken cancellationToken)
        {
            await notificationsRepository.AddNotificaiton(request.Notification, cancellationToken);
            await signalRNotifier.NewNotification(request.Notification, cancellationToken);
            return new Unit();
        }
    }
}
