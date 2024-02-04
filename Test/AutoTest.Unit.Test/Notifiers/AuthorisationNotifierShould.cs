using System.Threading;
using System.Threading.Tasks;
using AutoTest.Service.Interfaces;
using AutoTest.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Notifiers
{
    public class AuthorisationNotifierShould
    {
        private readonly IAuthorisationNotifier sut;
        private readonly MockRepository mr;
        private readonly Mock<IHubContext<AuthorisationHub>> eventHub;

        public AuthorisationNotifierShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            eventHub = mr.Create<IHubContext<AuthorisationHub>>();
            this.sut = new AuthorisationNotifier(eventHub.Object);
        }

        [Fact]
        public async Task AddEditableEntrant()
        {
            var eventId = 1ul;
            var email = "a@a.com";
            var clientProxy = mr.Create<IClientProxy>();
            var clients = mr.Create<IHubClients>();
            clients.Setup(a => a.Group($"email:{email}")).Returns(clientProxy.Object);
            eventHub.Setup(a => a.Clients).Returns(clients.Object);
            clientProxy.Setup(a => a.SendCoreAsync("AddEditableEntrant",
                new object[] { eventId },
                CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.AddEditableEntrant(eventId, new[] { email }, CancellationToken.None);

            mr.VerifyAll();
        }

        [Fact]
        public async Task AddEditableMarshal()
        {
            var eventId = 1ul;
            var email = "a@a.com";
            var clientProxy = mr.Create<IClientProxy>();
            var clients = mr.Create<IHubClients>();
            clients.Setup(a => a.Group($"email:{email}")).Returns(clientProxy.Object);
            eventHub.Setup(a => a.Clients).Returns(clients.Object);
            clientProxy.Setup(a => a.SendCoreAsync("AddEditableMarshal",
                new object[] { eventId },
                CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.AddEditableMarshal(eventId, new[] { email }, CancellationToken.None);

            mr.VerifyAll();
        }

        [Fact]
        public async Task NewClubAdmin()
        {
            var clubId = 1ul;
            var email = "a@a.com";
            var clientProxy = mr.Create<IClientProxy>();
            var clients = mr.Create<IHubClients>();
            clients.Setup(a => a.Group($"email:{email}")).Returns(clientProxy.Object);
            eventHub.Setup(a => a.Clients).Returns(clients.Object);
            clientProxy.Setup(a => a.SendCoreAsync("NewClubAdmin",
                new object[] { clubId },
                CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.NewClubAdmin(clubId, new[] { email }, CancellationToken.None);

            mr.VerifyAll();
        }

        [Fact]
        public async Task RemoveClubAdmin()
        {
            var clubId = 1ul;
            var email = "a@a.com";
            var clientProxy = mr.Create<IClientProxy>();
            var clients = mr.Create<IHubClients>();
            clients.Setup(a => a.Group($"email:{email}")).Returns(clientProxy.Object);
            eventHub.Setup(a => a.Clients).Returns(clients.Object);
            clientProxy.Setup(a => a.SendCoreAsync("RemoveClubAdmin",
                new object[] { clubId },
                CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.RemoveClubAdmin(clubId, new[] { email }, CancellationToken.None);

            mr.VerifyAll();
        }

        [Fact]
        public async Task NewEventMarshal()
        {
            var eventId = 1ul;
            var email = "a@a.com";
            var clientProxy = mr.Create<IClientProxy>();
            var clients = mr.Create<IHubClients>();
            clients.Setup(a => a.Group($"email:{email}")).Returns(clientProxy.Object);
            eventHub.Setup(a => a.Clients).Returns(clients.Object);
            clientProxy.Setup(a => a.SendCoreAsync("NewEventMarshal",
                new object[] { eventId },
                CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.NewEventMarshal(eventId, new[] { email }, CancellationToken.None);

            mr.VerifyAll();
        }

        [Fact]
        public async Task RemoveEventMarshal()
        {
            var eventId = 1ul;
            var email = "a@a.com";
            var clientProxy = mr.Create<IClientProxy>();
            var clients = mr.Create<IHubClients>();
            clients.Setup(a => a.Group($"email:{email}")).Returns(clientProxy.Object);
            eventHub.Setup(a => a.Clients).Returns(clients.Object);
            clientProxy.Setup(a => a.SendCoreAsync("RemoveEventMarshal",
                new object[] { eventId },
                CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.RemoveEventMarshal(eventId, new[] { email }, CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
