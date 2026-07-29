using System.Threading;
using System.Threading.Tasks;
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

public class ClubAdminOrSelfRequirementSelfHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
    private readonly AuthorizationHandler<ClubAdminOrSelfRequirement> _sut;

    public ClubAdminOrSelfRequirementSelfHandlerShould()
    {
        _mediator = _mr.Create<IMediator>();
        _httpContextAccessor = _mr.Create<IHttpContextAccessor>();
        _sut = new ClubAdminOrSelfRequirementSelfHandler(_httpContextAccessor.Object, _mediator.Object);
    }

    [Fact]
    public async Task ShouldPassIfEmailMatches()
    {
        var ac = AuthorizationHandlerContextFixture.GetAuthContext(
            new[] { new ClubAdminOrSelfRequirement() },
             "a@a.com");
        var entrantId = 99ul;
        var eventId = 1ul;
        var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", $"{eventId}"), ("entrantId", $"{entrantId}") });
        _httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx).Verifiable(Times.Once);
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEntrant(eventId, entrantId)), CancellationToken.None)).ReturnsAsync(Models.GetEntrant(eventId, entrantId)).Verifiable(Times.Once);

        await _sut.HandleAsync(ac);

        ac.HasSucceeded.Should().BeTrue();
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ShouldFailIfEmailsDontMatch()
    {
        var ac = AuthorizationHandlerContextFixture.GetAuthContext(
            new[] { new ClubAdminOrSelfRequirement() },
            "notA@a.com");
        var entrantId = 99ul;
        var eventId = 1ul;
        var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", $"{eventId}"), ("entrantId", $"{entrantId}") });
        _httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx).Verifiable(Times.Once);
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEntrant(eventId, entrantId)), CancellationToken.None)).ReturnsAsync(Models.GetEntrant(eventId, entrantId)
            ).Verifiable(Times.Once);

        await _sut.HandleAsync(ac);

        ac.HasSucceeded.Should().BeFalse();
        _mr.VerifyAll();
    }
}
