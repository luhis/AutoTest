using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using AutoTest.Service.Models;
using AutoTest.Unit.Test.MockData;
using AutoTest.Web.Hubs;
using AwesomeAssertions.ArgumentMatchers.Moq;
using Mediator;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Notifiers;

public class EventNotifierShould
{
    private readonly IEventNotifier _sut;
    private readonly MockRepository _mr;
    private readonly Mock<IHubContext<EventHub>> _eventHub;
    private readonly Mock<IMediator> _mediator;

    public EventNotifierShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
        _eventHub = _mr.Create<IHubContext<EventHub>>();
        _mediator = _mr.Create<IMediator>();
        _sut = new EventNotifier(_eventHub.Object, _mediator.Object);
    }

    [Fact]
    public async Task Notify()
    {
        var clients = _mr.Create<IHubClients>();
        var eventId = 2ul;
        var clientProxy = _mr.Create<IClientProxy>();
        var notification = new Notification(1, eventId, "test", new System.DateTime(2000, 1, 2), "admin");
        clientProxy.Setup(a => a.SendCoreAsync("NewNotification", new[] { notification }, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);
        clients.Setup(a => a.Group($"eventId:{eventId}")).Returns(clientProxy.Object);
        _eventHub.Setup(a => a.Clients).Returns(clients.Object);

        await _sut.NewNotification(notification, TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }

    [Fact]
    public async Task NewTestRun()
    {
        var clients = _mr.Create<IHubClients>();
        var eventId = 2ul;
        var clientProxy = _mr.Create<IClientProxy>();
        var testRun = new TestRun(1, eventId, 3, 60_000, 4, new System.DateTime(2000, 1, 2), 5);
        var results = new[] { new Result("A", new[] {
            new EntrantTimes(Models.GetEntrant(1, eventId), 55, new[] { new TestTime(1, System.Array.Empty<TestRun>()) } , 1, 1)}) };
        clientProxy.Setup(a => a.SendCoreAsync("NewResults", new[] { results }, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);
        clientProxy.Setup(a => a.SendCoreAsync("NewTestRun", new[] { testRun }, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);
        clients.Setup(a => a.Group($"eventId:{eventId}")).Returns(clientProxy.Object);
        _eventHub.Setup(a => a.Clients).Returns(clients.Object);
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetResults(eventId)), TestContext.Current.CancellationToken)).ReturnsAsync(results);

        await _sut.NewTestRun(testRun, TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }
}
