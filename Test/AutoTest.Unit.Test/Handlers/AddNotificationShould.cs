using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class AddNotificationShould
{
    private readonly IRequestHandler<AddNotification> _sut;
    private readonly MockRepository _mr;
    private readonly Mock<INotificationsRepository> _notificationsRepository;
    private readonly Mock<IEventNotifier> _notifier;

    public AddNotificationShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
        _notificationsRepository = _mr.Create<INotificationsRepository>();
        _notifier = _mr.Create<IEventNotifier>();
        _sut = new AddNotificationHandler(_notificationsRepository.Object, _notifier.Object);
    }

    [Fact]
    public async Task NotifyOnNewNotification()
    {
        var notification = new Notification(1, 2, "message", new System.DateTime(2000, 1, 1), "test user");
        _notifier.Setup(a => a.NewNotification(notification, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);
        _notificationsRepository.Setup(a => a.AddNotification(notification, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        await _sut.Handle(new(notification), TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }
}
