using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class AddNotificationShould
    {
        [Fact]
        public async Task NotifyOnNewNotification()
        {
            var mocker = new AutoMocker(MockBehavior.Strict);
            var sut = mocker.CreateInstance<AddNotificationHandler>();

            var notifier = mocker.GetMock<IEventNotifier>();
            var notification = new Notification(1, 2, "message", new System.DateTime(2000, 1, 1), "test user");
            notifier.Setup(a => a.NewNotification(notification, CancellationToken.None)).Returns(Task.CompletedTask);
            var notificationRepository = mocker.GetMock<INotificationsRepository>();
            notificationRepository.Setup(a => a.AddNotification(notification, CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new(notification), CancellationToken.None);

            mocker.VerifyAll();
        }
    }
}
