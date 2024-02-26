using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.Fixtures;
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
            var ac = AuthorizationHandlerContextFixture.GetAuthContext(
                new[] { new SelfRequirement() },
                 "a@a.com");
            var entrantId = 99ul;
            var eventId = 1ul;
            var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", $"{eventId}"), ("entrantId", $"{entrantId}") });
            httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx);
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEntrant(eventId, entrantId)), CancellationToken.None)).ReturnsAsync(
                new Entrant(entrantId, 1, "Joe", "Bloggs", "a@a.com", "A", eventId, Domain.Enums.Age.Senior, false));

            await sut.HandleAsync(ac);

            ac.HasSucceeded.Should().BeTrue();
            mr.VerifyAll();
        }

        [Fact]
        public async Task ShouldFailIfEmailsDontMatch()
        {
            var ac = AuthorizationHandlerContextFixture.GetAuthContext(
                new[] { new SelfRequirement() },
                "notA@a.com");
            var entrantId = 99ul;
            var eventId = 1ul;
            var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", $"{eventId}"), ("entrantId", $"{entrantId}") });
            httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx);
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEntrant(eventId, entrantId)), CancellationToken.None)).ReturnsAsync(
                new Entrant(entrantId, 1, "Joe", "Bloggs", "a@a.com", "A", eventId, Domain.Enums.Age.Senior, false));

            await sut.HandleAsync(ac);

            ac.HasSucceeded.Should().BeFalse();
            mr.VerifyAll();
        }
    }
}
