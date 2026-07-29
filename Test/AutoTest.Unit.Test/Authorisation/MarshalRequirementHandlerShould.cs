using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.Fixtures;
using AutoTest.Unit.Test.MockData;
using AutoTest.Web.Authorization.Attributes;
using AutoTest.Web.Authorization.Handlers;
using AwesomeAssertions;
using AwesomeAssertions.ArgumentMatchers.Moq;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Authorisation;

public class MarshalRequirementHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
    private readonly AuthorizationHandler<MarshalRequirement> _sut;

    public MarshalRequirementHandlerShould()
    {
        _mediator = _mr.Create<IMediator>();
        _httpContextAccessor = _mr.Create<IHttpContextAccessor>();
        _sut = new MarshalRequirementHandler(_httpContextAccessor.Object, _mediator.Object);
    }

    [Fact]
    public async Task ShouldPassIfEmailMatches()
    {
        var ac = AuthorizationHandlerContextFixture.GetAuthContext(new[] { new MarshalRequirement() }, "marshal@email.com");
        var eventId = 1ul;
        var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", $"{eventId}") });
        _httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx).Verifiable(Times.Once);
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEvent(eventId)), CancellationToken.None)).ReturnsAsync(Models.GetEvent(eventId)).Verifiable(Times.Once);
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetMarshals(eventId)), CancellationToken.None))
            .ReturnsAsync(new[] { new Marshal(1, "Joe", "Marshall", "marshal@email.com", eventId, 123456, "") }).Verifiable(Times.Once);


        await _sut.HandleAsync(ac);

        ac.HasSucceeded.Should().BeTrue();
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ThrowIfNoEvent()
    {
        var ac = AuthorizationHandlerContextFixture.GetAuthContext(new[] { new MarshalRequirement() }, "marshal@email.com");
        var eventId = 1ul;
        var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", $"{eventId}") });
        _httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx).Verifiable(Times.Once);
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEvent(eventId)), CancellationToken.None)).ReturnsAsync(
            (Event?)null).Verifiable(Times.Once);

        await _sut.HandleAsync(ac);

        ac.HasFailed.Should().BeTrue();
        ac.FailureReasons.Should().BeEquivalentTo([new AuthorizationFailureReason(_sut, "Cannot find event")]);
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ShouldFailIfEmailsDontMatch()
    {
        var ac = AuthorizationHandlerContextFixture.GetAuthContext(new[] { new MarshalRequirement() }, "NotMarshal@email.com");
        var eventId = 1ul;
        var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", $"{eventId}") });
        _httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx).Verifiable(Times.Once);
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEvent(eventId)), CancellationToken.None)).ReturnsAsync(
            Models.GetEvent(eventId)).Verifiable(Times.Once);
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetMarshals(eventId)), CancellationToken.None))
            .ReturnsAsync(new[] { new Marshal(1, "Joe", "Marshall", "marshal@email.com", eventId, 123456, "") }).Verifiable(Times.Once);


        await _sut.HandleAsync(ac);

        ac.HasSucceeded.Should().BeFalse();
        _mr.VerifyAll();
    }
}
