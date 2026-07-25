using System;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization;
using AwesomeAssertions;
using AwesomeAssertions.ArgumentMatchers.Moq;
using Mediator;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Authorisation;

public class AuthToolsShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IMediator> _mediator;

    public AuthToolsShould()
    {
        _mediator = _mr.Create<IMediator>();
    }

    [Fact]
    public void GetEventId()
    {
        var rd = new RouteData(new RouteValueDictionary());
        rd.Values.Add("eventId", $"{1}");
        var res = AuthTools.GetEventId(rd);

        res.Should().Be(1ul);
    }

    [Fact]
    public void FailGetEventIdWhenNotPresent()
    {
        var rd = new RouteData(new RouteValueDictionary());

        Action act = () => AuthTools.GetEventId(rd);

        act.Should().Throw<Exception>().WithMessage("Don't know how to get EventId from this request");
    }

    [Fact]
    public async Task FailGetEmailWhenNotPresentAsync()
    {
        var rd = new RouteData(new RouteValueDictionary());

        Func<Task<string?>> act = () => AuthTools.GetExistingEmail(rd, _mediator.Object, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>().WithMessage("Don't know how to get Email from this request");
        _mr.VerifyAll();
    }

    [Fact]
    public async Task GetFromEntrantId()
    {
        var eventId = 1ul;
        var entrantId = 2ul;
        var rd = new RouteData(new RouteValueDictionary());
        rd.Values.Add("eventId", $"{eventId}");
        rd.Values.Add("entrantId", $"{entrantId}");
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEntrant(eventId, entrantId)), TestContext.Current.CancellationToken)).ReturnsAsync(
            new Entrant(entrantId, 22, "Joe", "Bloggs", "a@a.com", "A", 99, Domain.Enums.Age.Senior, false, null));

        var email = await AuthTools.GetExistingEmail(rd, _mediator.Object, TestContext.Current.CancellationToken);

        email.Should().Be("a@a.com");
        _mr.VerifyAll();
    }

    [Fact]
    public async Task GetFromEntrantIdNotFound()
    {
        var eventId = 1ul;
        var entrantId = 2ul;
        var rd = new RouteData(new RouteValueDictionary());
        rd.Values.Add("eventId", $"{eventId}");
        rd.Values.Add("entrantId", $"{entrantId}");
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEntrant(eventId, entrantId)), TestContext.Current.CancellationToken)).ReturnsAsync((Entrant?)
            null);

        var email = await AuthTools.GetExistingEmail(rd, _mediator.Object, TestContext.Current.CancellationToken);

        email.Should().BeNull();
        _mr.VerifyAll();
    }

    [Fact]
    public async Task GetFromMarshalId()
    {
        var eventId = 1ul;
        var marshalId = 2ul;
        var rd = new RouteData(new RouteValueDictionary());
        rd.Values.Add("eventId", $"{eventId}");
        rd.Values.Add("marshalId", $"{marshalId}");
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetMarshal(eventId, marshalId)), TestContext.Current.CancellationToken)).ReturnsAsync(
            new Marshal(marshalId, "Joe", "Bloggs", "a@a.com", eventId, 9876543, "role"));

        var email = await AuthTools.GetExistingEmail(rd, _mediator.Object, TestContext.Current.CancellationToken);

        email.Should().Be("a@a.com");
        _mr.VerifyAll();
    }

    [Fact]
    public async Task GetFromMarshalIdNotFound()
    {
        var eventId = 1ul;
        var marshalId = 2ul;
        var rd = new RouteData(new RouteValueDictionary());
        rd.Values.Add("eventId", $"{eventId}");
        rd.Values.Add("marshalId", $"{marshalId}");
        _mediator.Setup(a => a.Send(Its.EquivalentTo(new GetMarshal(eventId, marshalId)), TestContext.Current.CancellationToken)).ReturnsAsync((Marshal?)null);

        var email = await AuthTools.GetExistingEmail(rd, _mediator.Object, TestContext.Current.CancellationToken);

        email.Should().BeNull();
        _mr.VerifyAll();
    }
}
