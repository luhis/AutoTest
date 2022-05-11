using System;
using System.Threading.Tasks;
using AutoTest.Web.Authorization;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Authorisation
{
    public class AuthToolsShould
    {

        private readonly MockRepository mr;
        private readonly Mock<IMediator> mediator;

        public AuthToolsShould()
        {
            this.mr = new MockRepository(MockBehavior.Strict);
            this.mediator = mr.Create<IMediator>();
        }

        [Fact]
        public void GetEventId()
        {
            var rd = new RouteData(new RouteValueDictionary());
            rd.Values.Add("eventId", 1.ToString());
            var res = AuthTools.GetEventId(rd);

            res.Should().Be(1ul);
        }

        [Fact]
        public void FailGetEventIdWhenNotPresent()
        {
            var rd = new RouteData(new RouteValueDictionary());

            Action act = () => AuthTools.GetEventId(rd);

            act.Should().Throw<Exception>();
        }

//        [Fact]
//        public void FailGetEmailWhenNotPresent()
//        {
//            var rd = new RouteData(new RouteValueDictionary());

//            Task<string> act = AuthTools.GetEmail(rd, mediator.Object);

//#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
//            act.Result.Should().Throw<Exception>();
//#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
//            mr.VerifyAll();
//        }
    }
}
