using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.Fixtures;
using AutoTest.Unit.Test.MockData;
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
    public class MarshalRequirementHandlerShould
    {
        private readonly AuthorizationHandler<MarshalRequirement> sut;
        private readonly MockRepository mr;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IHttpContextAccessor> httpContextAccessor;

        public MarshalRequirementHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            mediator = mr.Create<IMediator>();
            httpContextAccessor = mr.Create<IHttpContextAccessor>();
            sut = new MarshalRequirementHandler(httpContextAccessor.Object, mediator.Object);
        }

        [Fact]
        public async Task ShouldPassIfEmailMatches()
        {
            var ac = AuthorizationHandlerContextFixture.GetAuthContext(new[] { new MarshalRequirement() }, "marshal@email.com");
            var eventId = 1ul;
            var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", $"{eventId}") });
            httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx);
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEvent(eventId)), CancellationToken.None)).ReturnsAsync(Models.GetEvent(eventId));
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetMarshals(eventId)), CancellationToken.None))
                .ReturnsAsync(new[] { new Marshal(1, "Joe", "Marshall", "marshal@email.com", eventId, 123456, "") });


            await sut.HandleAsync(ac);

            ac.HasSucceeded.Should().BeTrue();
            mr.VerifyAll();
        }

        [Fact]
        public async Task ThrowIfNoEvent()
        {
            var ac = AuthorizationHandlerContextFixture.GetAuthContext(new[] { new MarshalRequirement() }, "marshal@email.com");
            var eventId = 1ul;
            var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", $"{eventId}") });
            httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx);
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEvent(eventId)), CancellationToken.None)).ReturnsAsync(
                (Event?)null);

            await sut.HandleAsync(ac);

            ac.HasFailed.Should().BeTrue();
            ac.FailureReasons.Should().BeEquivalentTo([new AuthorizationFailureReason(sut, "Cannot find event")]);
            mr.VerifyAll();
        }

        [Fact]
        public async Task ShouldFailIfEmailsDontMatch()
        {
            var ac = AuthorizationHandlerContextFixture.GetAuthContext(new[] { new MarshalRequirement() }, "NotMarshal@email.com");
            var eventId = 1ul;
            var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", $"{eventId}") });
            httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx);
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEvent(eventId)), CancellationToken.None)).ReturnsAsync(
                Models.GetEvent(eventId));
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetMarshals(eventId)), CancellationToken.None))
                .ReturnsAsync(new[] { new Marshal(1, "Joe", "Marshall", "marshal@email.com", eventId, 123456, "") });


            await sut.HandleAsync(ac);

            ac.HasSucceeded.Should().BeFalse();
            mr.VerifyAll();
        }
    }
}
