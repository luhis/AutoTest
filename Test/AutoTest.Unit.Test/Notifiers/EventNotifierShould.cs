using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using AutoTest.Service.Models;
using AutoTest.Web.Hubs;
using FluentAssertions.ArgumentMatchers.Moq;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Notifiers
{
    public class EventNotifierShould
    {
        private readonly IEventNotifier sut;
        private readonly MockRepository mr;
        private readonly Mock<IHubContext<EventHub>> eventHub;
        private readonly Mock<IMediator> mediator;

        public EventNotifierShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            eventHub = mr.Create<IHubContext<EventHub>>();
            mediator = mr.Create<IMediator>();
            this.sut = new EventNotifier(eventHub.Object, mediator.Object);
        }

        [Fact]
        public async Task Notify()
        {
            var clients = mr.Create<IHubClients>();
            var eventId = 2ul;
            var clientProxy = mr.Create<IClientProxy>();
            var notification = new Notification(1, eventId, "test", new System.DateTime(2000, 1, 2), "admin");
            clientProxy.Setup(a => a.SendCoreAsync("NewNotification", new[] { notification }, CancellationToken.None)).Returns(Task.CompletedTask);
            clients.Setup(a => a.Group($"eventId:{eventId}")).Returns(clientProxy.Object);
            eventHub.Setup(a => a.Clients).Returns(clients.Object);

            await sut.NewNotification(notification, CancellationToken.None);

            mr.VerifyAll();
        }

        [Fact]
        public async Task NewTestRun()
        {
            var clients = mr.Create<IHubClients>();
            var eventId = 2ul;
            var clientProxy = mr.Create<IClientProxy>();
            var testRun = new TestRun(1, eventId, 3, 60_000, 4, new System.DateTime(2000, 1, 2), 5);
            var results = new[] { new Result("A", new[] {
                new EntrantTimes(new Entrant(1, 2, "given", "last", "a@a.com", Domain.Enums.EventType.AutoTest, "A", eventId, "BRMC", 123456, Domain.Enums.Age.Senior, false), 55, new[] { new TestTime(1, System.Array.Empty<TestRun>()) } , 1, 1)}) };
            clientProxy.Setup(a => a.SendCoreAsync("NewResults", new[] { results }, CancellationToken.None)).Returns(Task.CompletedTask);
            clientProxy.Setup(a => a.SendCoreAsync("NewTestRun", new[] { testRun }, CancellationToken.None)).Returns(Task.CompletedTask);
            clients.Setup(a => a.Group($"eventId:{eventId}")).Returns(clientProxy.Object);
            eventHub.Setup(a => a.Clients).Returns(clients.Object);
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetResults(eventId)), CancellationToken.None)).ReturnsAsync(results);

            await sut.NewTestRun(testRun, CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
