using System.Threading;
using System.Threading.Tasks;
using AutoTest.Service.Interfaces;
using AutoTest.Web.Hubs;
using FluentAssertions.ArgumentMatchers.Moq;
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
        public async Task Test()
        {
            var entrantId = 1ul;
            var email = "a@a.com";
            var clientProxy = mr.Create<IClientProxy>();
            var clients = mr.Create<IHubClients>();
            clients.Setup(a => a.Group($"email:{email}")).Returns(clientProxy.Object);
            eventHub.Setup(a => a.Clients).Returns(clients.Object);
            clientProxy.Setup(a => a.SendCoreAsync("AddEditableEntrant",
                It.IsAny<object[]>(),
                //Its.EquivalentTo(new object[] { entrantId, It.IsAny<string[]>() }),//Its.EquivalentTo(new[] { email }) }),
                CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.AddEditableEntrant(entrantId, new[] { email }, CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
