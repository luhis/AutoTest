using System.Threading;
using System.Threading.Tasks;
using AutoTest.Service.Interfaces;
using AutoTest.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Notifiers;

public class AuthorisationNotifierShould
{
    private readonly IAuthorisationNotifier sut;
    private readonly MockRepository mr;
    private readonly Mock<IHubContext<AuthorisationHub>> eventHub;

    public AuthorisationNotifierShould()
    {
        mr = new MockRepository(MockBehavior.Strict);
        eventHub = mr.Create<IHubContext<AuthorisationHub>>();
        sut = new AuthorisationNotifier(eventHub.Object);
    }

    private void SetupHubSend(string groupName, string methodName, object[] args)
    {
        var clientProxy = mr.Create<IClientProxy>();
        var clients = mr.Create<IHubClients>();
        clients.Setup(a => a.Group(groupName)).Returns(clientProxy.Object);
        eventHub.Setup(a => a.Clients).Returns(clients.Object);
        clientProxy.Setup(a => a.SendCoreAsync(methodName, args, CancellationToken.None)).Returns(Task.CompletedTask);
    }

    [Fact]
    public async Task AddEditableEntrant()
    {
        var eventId = 1ul;
        var email = "a@a.com";
        SetupHubSend($"email:{email}", "AddEditableEntrant", new object[] { eventId });

        await sut.AddEditableEntrant(eventId, new[] { email }, CancellationToken.None);

        mr.VerifyAll();
    }

    [Fact]
    public async Task AddEditableMarshal()
    {
        var eventId = 1ul;
        var email = "a@a.com";
        SetupHubSend($"email:{email}", "AddEditableMarshal", new object[] { eventId });

        await sut.AddEditableMarshal(eventId, new[] { email }, CancellationToken.None);

        mr.VerifyAll();
    }

    [Fact]
    public async Task NewClubAdmin()
    {
        var clubId = 1ul;
        var email = "a@a.com";
        SetupHubSend($"email:{email}", "NewClubAdmin", new object[] { clubId });

        await sut.NewClubAdmin(clubId, new[] { email }, CancellationToken.None);

        mr.VerifyAll();
    }

    [Fact]
    public async Task RemoveClubAdmin()
    {
        var clubId = 1ul;
        var email = "a@a.com";
        SetupHubSend($"email:{email}", "RemoveClubAdmin", new object[] { clubId });

        await sut.RemoveClubAdmin(clubId, new[] { email }, CancellationToken.None);

        mr.VerifyAll();
    }

    [Fact]
    public async Task NewEventMarshal()
    {
        var eventId = 1ul;
        var email = "a@a.com";
        SetupHubSend($"email:{email}", "NewEventMarshal", new object[] { eventId });

        await sut.NewEventMarshal(eventId, new[] { email }, CancellationToken.None);

        mr.VerifyAll();
    }

    [Fact]
    public async Task RemoveEventMarshal()
    {
        var eventId = 1ul;
        var email = "a@a.com";
        SetupHubSend($"email:{email}", "RemoveEventMarshal", new object[] { eventId });

        await sut.RemoveEventMarshal(eventId, new[] { email }, CancellationToken.None);

        mr.VerifyAll();
    }
}
