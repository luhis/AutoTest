using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AwesomeAssertions;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class GetNotificationsHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<INotificationsRepository> _notificationsRepository;
    private readonly IRequestHandler<GetNotifications, IEnumerable<Notification>> _sut;

    public GetNotificationsHandlerShould()
    {
        _notificationsRepository = _mr.Create<INotificationsRepository>();
        _sut = new GetNotificationsHandler(_notificationsRepository.Object);
    }

    [Fact]
    public async Task ReturnNotifications()
    {
        var eventId = 1ul;
        var notifications = new[]
        {
            new Notification(1, eventId, "Test message", new System.DateTime(2000, 1, 1), "admin@test.com"),
        };
        _notificationsRepository.Setup(a => a.GetNotifications(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(notifications).Verifiable(Times.Once);

        var res = (await _sut.Handle(new(eventId), TestContext.Current.CancellationToken)).ToArray();

        res.Should().BeEquivalentTo(notifications);
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ReturnEmptyWhenNoNotifications()
    {
        var eventId = 1ul;
        _notificationsRepository.Setup(a => a.GetNotifications(eventId, TestContext.Current.CancellationToken)).ReturnsAsync(Enumerable.Empty<Notification>()).Verifiable(Times.Once);

        var res = (await _sut.Handle(new(eventId), TestContext.Current.CancellationToken)).ToArray();

        res.Should().BeEmpty();
        _mr.VerifyAll();
    }
}
