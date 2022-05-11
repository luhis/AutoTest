using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization.Attributes;
using AutoTest.Web.Authorization.Handlers;
using FluentAssertions;
using FluentAssertions.ArgumentMatchers.Moq;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Authorisation
{
    public class SelfRequirementHandlerShould
    {
        private readonly AuthorizationHandler<SelfRequirement> sut;
        private readonly MockRepository mr;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IHttpContextAccessor> httpContextAccessor;

        public SelfRequirementHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            mediator = mr.Create<IMediator>();
            httpContextAccessor = mr.Create<IHttpContextAccessor>();
            sut = new SelfRequirementHandler(httpContextAccessor.Object, mediator.Object);
        }

        [Fact]
        public async Task ShouldPassIfEmailMatches()
        {
            var ac = new AuthorizationHandlerContext(
                new[] { new SelfRequirement() },
                new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, "a@a.com") })), null);
            var ctx = new DefaultHttpContext();
            var entrantId = 99ul;
            var eventId = 1ul;
            ctx.Request.RouteValues.Add("eventId", eventId.ToString());
            ctx.Request.RouteValues.Add("entrantId", entrantId.ToString());
            httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx);
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEntrant(eventId, entrantId)), CancellationToken.None)).Returns(Task.FromResult<Entrant?>(
                new Entrant(entrantId, 1, "Joe", "Bloggs", "a@a.com", "A", eventId, "BRMC", 12345678, Domain.Enums.Age.Senior)));

            await sut.HandleAsync(ac);

            ac.HasSucceeded.Should().BeTrue();
            mr.VerifyAll();
        }

        [Fact]
        public async Task ShouldFailIfEmailsDontMatch()
        {
            var ac = new AuthorizationHandlerContext(
                new[] { new SelfRequirement() },
                new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, "notA@a.com") })), null);
            var ctx = new DefaultHttpContext();
            var entrantId = 99ul;
            var eventId = 1ul;
            ctx.Request.RouteValues.Add("eventId", eventId.ToString());
            ctx.Request.RouteValues.Add("entrantId", entrantId.ToString());
            httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx);
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEntrant(eventId, entrantId)), CancellationToken.None)).Returns(Task.FromResult<Entrant?>(
                new Entrant(entrantId, 1, "Joe", "Bloggs", "a@a.com", "A", eventId, "BRMC", 12345678, Domain.Enums.Age.Senior)));

            await sut.HandleAsync(ac);

            ac.HasSucceeded.Should().BeFalse();
            mr.VerifyAll();
        }
    }
}
