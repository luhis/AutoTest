using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AwesomeAssertions;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class GetAccessHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly RootAdminEmails _rootAdminEmails = RootAdminEmails.FromConfig(["admin@test.com"]);
    private readonly Mock<IServiceScopeFactory> _scopeFactory;
    private readonly Mock<IServiceScope> _scope;
    private readonly Mock<IMediator> _mediator;
    private readonly IRequestHandler<GetAccess, AccessModel> _sut;

    public GetAccessHandlerShould()
    {
        _scopeFactory = _mr.Create<IServiceScopeFactory>();
        _scope = _mr.Create<IServiceScope>();
        _mediator = _mr.Create<IMediator>();

        _scopeFactory.Setup(a => a.CreateScope()).Returns(_scope.Object);
        _scope.Setup(a => a.ServiceProvider).Returns(Mock.Of<IServiceProvider>(sp =>
            sp.GetService(typeof(IMediator)) == _mediator.Object));
        _scope.Setup(a => a.Dispose());

        _sut = new GetAccessHandler(_rootAdminEmails, _scopeFactory.Object);
    }

    [Fact]
    public async Task ReturnUnauthorisedAccess()
    {
        var email = "user@test.com";
        var request = new GetAccess(email, false);

        SetupSubHandlers(email, [], [], [], []);

        var result = await _sut.Handle(request, CancellationToken.None);

        result.Should().BeEquivalentTo(new AccessModel(false, false, [], [], [], []));
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ReturnAuthorisedAccess()
    {
        var email = "user@test.com";
        var request = new GetAccess(email, true);

        SetupSubHandlers(email, [1, 2], [3], [4, 5], [6]);

        var result = await _sut.Handle(request, CancellationToken.None);

        result.Should().BeEquivalentTo(new AccessModel(false, true, [1, 2], [3], [4, 5], [6]));
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ReturnRootAdminAccess()
    {
        var email = "admin@test.com";
        var request = new GetAccess(email, true);

        SetupSubHandlers(email, [1], [2], [3], [4]);

        var result = await _sut.Handle(request, CancellationToken.None);

        result.Should().BeEquivalentTo(new AccessModel(true, true, [1], [2], [3], [4]));
        _mr.VerifyAll();
    }

    private void SetupSubHandlers(string email, ulong[] adminClubs, ulong[] marshalEvents, ulong[] editableEntrants, ulong[] editableMarshals)
    {
        _mediator.Setup(a => a.Send(It.Is<GetAdminClubs>(m => m.EmailAddress == email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(adminClubs);
        _mediator.Setup(a => a.Send(It.Is<GetMarshalEvents>(m => m.EmailAddress == email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(marshalEvents);
        _mediator.Setup(a => a.Send(It.Is<GetEditableEntrants>(m => m.EmailAddress == email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(editableEntrants);
        _mediator.Setup(a => a.Send(It.Is<GetEditableMarshals>(m => m.EmailAddress == email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(editableMarshals);
    }
}
