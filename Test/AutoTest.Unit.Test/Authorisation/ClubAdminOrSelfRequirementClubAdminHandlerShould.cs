using System;
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
    public class ClubAdminOrSelfRequirementClubAdminHandlerShould
    {
        private readonly AuthorizationHandler<ClubAdminOrSelfRequirement> sut;
        private readonly MockRepository mr;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IHttpContextAccessor> httpContextAccessor;

        public ClubAdminOrSelfRequirementClubAdminHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            mediator = mr.Create<IMediator>();
            httpContextAccessor = mr.Create<IHttpContextAccessor>();
            sut = new ClubAdminOrSelfRequirementClubAdminHandler(httpContextAccessor.Object, mediator.Object);
        }

        [Fact]
        public async Task ShouldPassIfEmailMatches()
        {
            var ac = AuthorizationHandlerContextFixture.GetAuthContext(
                new[] { new ClubAdminOrSelfRequirement() },
                 "a@a.com");
            var entrantId = 99ul;
            var eventId = 1ul;
            var clubId = 88ul;
            var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", eventId.ToString()), ("entrantId", entrantId.ToString()) });
            httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx);
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEvent(eventId)), CancellationToken.None)).ReturnsAsync(
                Models.GetEvent(eventId, clubId));
            var club = new Club(clubId, "BRMC", "pay@brmc.org", "www.com");
            club.AdminEmails.Add(new("a@a.com"));
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetClub(clubId)), CancellationToken.None)).ReturnsAsync(
                club);

            await sut.HandleAsync(ac);

            ac.HasSucceeded.Should().BeTrue();
            mr.VerifyAll();
        }

        [Fact]
        public async Task ShouldPassIfNewEvent()
        {
            var ac = AuthorizationHandlerContextFixture.GetAuthContext(
                new[] { new ClubAdminOrSelfRequirement() },
                 "a@a.com");
            var entrantId = 99ul;
            var eventId = 1ul;
            var clubId = 88ul;
            var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", eventId.ToString()), ("entrantId", entrantId.ToString()) });
            httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx);
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEvent(eventId)), CancellationToken.None)).ReturnsAsync((Event?)null);
            var club = new Club(clubId, "BRMC", "pay@brmc.org", "www.com");
            club.AdminEmails.Add(new("a@a.com"));

            await sut.HandleAsync(ac);

            ac.HasSucceeded.Should().BeTrue();
            mr.VerifyAll();
        }

        [Fact]
        public async Task ShouldFailIfEmailsDontMatch()
        {
            var ac = AuthorizationHandlerContextFixture.GetAuthContext(
                new[] { new ClubAdminOrSelfRequirement() },
                "notA@a.com");
            var entrantId = 99ul;
            var eventId = 1ul;
            var clubId = 88ul;
            var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", eventId.ToString()), ("entrantId", entrantId.ToString()) });
            httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx);
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEvent(eventId)), CancellationToken.None)).ReturnsAsync(
                Models.GetEvent(eventId, clubId));
            var club = new Club(clubId, "BRMC", "pay@brmc.org", "www.com");
            club.AdminEmails.Add(new("a@a.com"));
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetClub(clubId)), CancellationToken.None)).ReturnsAsync(
                club);
            await sut.HandleAsync(ac);

            ac.HasSucceeded.Should().BeFalse();
            mr.VerifyAll();
        }

        [Fact]
        public async Task ShouldFailIfClubNotFound()
        {
            var ac = AuthorizationHandlerContextFixture.GetAuthContext(
                new[] { new ClubAdminOrSelfRequirement() },
                 "a@a.com");
            var entrantId = 99ul;
            var eventId = 1ul;
            var clubId = 88ul;
            var ctx = HttpContextFixture.GetHttpContext(new[] { ("eventId", eventId.ToString()), ("entrantId", entrantId.ToString()) });
            httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx);
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEvent(eventId)), CancellationToken.None)).ReturnsAsync(
                Models.GetEvent(eventId, clubId));
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetClub(clubId)), CancellationToken.None)).ReturnsAsync(
                (Club?)null);

            Func<Task> a = () => sut.HandleAsync(ac);

            await a.Should().ThrowAsync<NullReferenceException>().WithMessage("club");
            mr.VerifyAll();
        }
    }
}
