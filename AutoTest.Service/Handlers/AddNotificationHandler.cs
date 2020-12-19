using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class AddNotificationHandler : IRequestHandler<AddNotification, Unit>
    {
        private readonly AutoTestContext autoTestContext;
        private readonly ISignalRNotifier signalRNotifier;

        public AddNotificationHandler(AutoTestContext autoTestContext, ISignalRNotifier signalRNotifier)
        {
            this.autoTestContext = autoTestContext;
            this.signalRNotifier = signalRNotifier;
        }

        async Task<Unit> IRequestHandler<AddNotification, Unit>.Handle(AddNotification request, CancellationToken cancellationToken)
        {
            await autoTestContext.Notifications!.Upsert(request.Notification, a => a.NotificationId == request.Notification.NotificationId, cancellationToken);
            await this.autoTestContext.SaveChangesAsync(cancellationToken);
            await signalRNotifier.NewNotification(request.Notification, cancellationToken);
            return new Unit();
        }
    }
}
