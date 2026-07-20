using System;
using System.Collections.Generic;
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

    public static IEnumerable<object[]> NotifierTestData => new[]
    {
        new object[] { "AddEditableEntrant", 1ul, (Func<IAuthorisationNotifier, ulong, IEnumerable<string>, CancellationToken, Task>)((n, id, e, ct) => n.AddEditableEntrant(id, e, ct)) },
        new object[] { "AddEditableMarshal", 1ul, (Func<IAuthorisationNotifier, ulong, IEnumerable<string>, CancellationToken, Task>)((n, id, e, ct) => n.AddEditableMarshal(id, e, ct)) },
        new object[] { "NewClubAdmin", 1ul, (Func<IAuthorisationNotifier, ulong, IEnumerable<string>, CancellationToken, Task>)((n, id, e, ct) => n.NewClubAdmin(id, e, ct)) },
        new object[] { "RemoveClubAdmin", 1ul, (Func<IAuthorisationNotifier, ulong, IEnumerable<string>, CancellationToken, Task>)((n, id, e, ct) => n.RemoveClubAdmin(id, e, ct)) },
        new object[] { "NewEventMarshal", 1ul, (Func<IAuthorisationNotifier, ulong, IEnumerable<string>, CancellationToken, Task>)((n, id, e, ct) => n.NewEventMarshal(id, e, ct)) },
        new object[] { "RemoveEventMarshal", 1ul, (Func<IAuthorisationNotifier, ulong, IEnumerable<string>, CancellationToken, Task>)((n, id, e, ct) => n.RemoveEventMarshal(id, e, ct)) },
    };

    [Theory]
    [MemberData(nameof(NotifierTestData))]
    public async Task SendsSignalRMessageToCorrectMethod(string methodName, ulong id, Func<IAuthorisationNotifier, ulong, IEnumerable<string>, CancellationToken, Task> invoke)
    {
        SetupHubSend("email:a@a.com", methodName, new object[] { id });

        await invoke(sut, id, new[] { "a@a.com" }, CancellationToken.None);

        mr.VerifyAll();
    }
}
