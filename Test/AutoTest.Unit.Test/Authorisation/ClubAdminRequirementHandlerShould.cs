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

public class ClubAdminRequirementHandlerShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
    private readonly AuthorizationHandler<ClubAdminRequirement> _sut;

    public ClubAdminRequirementHandlerShould()
    {
        _mediator = _mr.Create<IMediator>();
        _httpContextAccessor = _mr.Create<IHttpContextAccessor>();
        _sut = new ClubAdminRequirementHandler(_httpContextAccessor.Object, _mediator.Object);
    }

    [Fact]
    public async Task ShouldPassIfEmailMatches()
    {
        var ac = AuthorizationHandlerContextFixture.GetAuthContext(
            new[] { new ClubAdminRequirement() },
             "a@a.com");
        var entrantId = 99ul;
        var eventId = 1ul;
        var clubId = 88ul;
        var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", $"{eventId}"), ("entrantId", $"{entrantId}") });
        _httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx);
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEvent(eventId)), CancellationToken.None)).ReturnsAsync(
            Models.GetEvent(eventId, clubId));
        var club = new Club(clubId, "BRMC", "pay@brmc.org", "www.com");
        club.AdminEmails.Add(new("a@a.com"));
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetClub(clubId)), CancellationToken.None)).ReturnsAsync(
            club);

        await _sut.HandleAsync(ac);

        ac.HasSucceeded.Should().BeTrue();
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ShouldPassIfNewEvent()
    {
        var ac = AuthorizationHandlerContextFixture.GetAuthContext(
            new[] { new ClubAdminRequirement() },
             "a@a.com");
        var entrantId = 99ul;
        var eventId = 1ul;
        var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", $"{eventId}"), ("entrantId", $"{entrantId}") });
        _httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx);
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEvent(eventId)), CancellationToken.None)).ReturnsAsync((Event?)null);

        await _sut.HandleAsync(ac);

        ac.HasSucceeded.Should().BeTrue();
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ShouldThrowIfClubNotFound()
    {
        var ac = AuthorizationHandlerContextFixture.GetAuthContext(
            new[] { new ClubAdminRequirement() },
            "notA@a.com");
        var entrantId = 99ul;
        var eventId = 1ul;
        var clubId = 88ul;
        var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", $"{eventId}"), ("entrantId", $"{entrantId}") });
        _httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx);
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEvent(eventId)), CancellationToken.None)).ReturnsAsync(
            Models.GetEvent(eventId, clubId));
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetClub(clubId)), CancellationToken.None)).ReturnsAsync(
            (Club?)null);
        await _sut.HandleAsync(ac);

        ac.HasFailed.Should().BeTrue();
        ac.FailureReasons.Should().BeEquivalentTo([new AuthorizationFailureReason(_sut, "Club not found")]);
        _mr.VerifyAll();
    }

    [Fact]
    public async Task ShouldFailIfEmailsDontMatch()
    {
        var ac = AuthorizationHandlerContextFixture.GetAuthContext(
            new[] { new ClubAdminRequirement() },
            "notA@a.com");
        var entrantId = 99ul;
        var eventId = 1ul;
        var clubId = 88ul;
        var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", $"{eventId}"), ("entrantId", $"{entrantId}") });
        _httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx);
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEvent(eventId)), CancellationToken.None)).ReturnsAsync(
            Models.GetEvent(eventId, clubId));
        var club = new Club(clubId, "BRMC", "pay@brmc.org", "www.com");
        club.AdminEmails.Add(new("a@a.com"));
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetClub(clubId)), CancellationToken.None)).ReturnsAsync(
            club);
        await _sut.HandleAsync(ac);

        ac.HasSucceeded.Should().BeFalse();
        _mr.VerifyAll();
    }
}
