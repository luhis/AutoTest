using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization.Attributes;
using AutoTest.Web.Authorization.Handlers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
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
        public async Task ShouldPassIfSelf()
        {
            var ac = new AuthorizationHandlerContext(new[] { new SelfRequirement() }, new System.Security.Claims.ClaimsPrincipal(), null);
            var ctx = new DefaultHttpContext();
            var entrantId = 99ul;
            ctx.Request.RouteValues.Add("eventId", "1");
            ctx.Request.RouteValues.Add("entrantId", entrantId.ToString());
            httpContextAccessor.SetupGet(a => a.HttpContext).Returns(ctx);
            mediator.Setup(a => a.Send(It.Is<GetEntrant>(e => e.EntrantId == entrantId), CancellationToken.None)).Returns(Task.FromResult<Entrant?>(
                new Entrant(entrantId, 1, "Joe", "Bloggs", "a@a.com", "A", 88, "BRMC", 12345678, Domain.Enums.Age.Senior)));

            await sut.HandleAsync(ac);

            //ac.HasSucceeded.Should().BeTrue(); // TODO
            mr.VerifyAll();
        }

        //[Fact]
        //public async Task ShouldFailIfEmptyRouteData()
        //{
        //    var ac = new AuthorizationHandlerContext(new[] { new SelfRequirement() }, new System.Security.Claims.ClaimsPrincipal(), null);
        //    httpContextAccessor.SetupGet(a => a.HttpContext).Returns((HttpContext?)null);

        //    await sut.HandleAsync(ac);

        //    mr.VerifyAll();
        //}
    }
}
