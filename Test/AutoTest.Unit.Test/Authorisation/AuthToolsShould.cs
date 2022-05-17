using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using AutoTest.Web.Authorization;
using FluentAssertions;
using FluentAssertions.ArgumentMatchers.Moq;
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

            act.Should().Throw<Exception>().WithMessage("Don't know how to get EventId from this request");
        }

        [Fact]
        public async Task FailGetEmailWhenNotPresentAsync()
        {
            var rd = new RouteData(new RouteValueDictionary());

            Func<Task<string>> act = () => AuthTools.GetEmail(rd, mediator.Object);

            await act.Should().ThrowAsync<Exception>().WithMessage("Don't know how to get Email from this request");
            mr.VerifyAll();
        }

        [Fact]
        public async Task GetFromEntrantId()
        {
            var eventId = 1ul;
            var entrantId = 2ul;
            var rd = new RouteData(new RouteValueDictionary());
            rd.Values.Add("eventId", eventId.ToString());
            rd.Values.Add("entrantId", entrantId.ToString());
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetEntrant(eventId, entrantId)), CancellationToken.None)).ReturnsAsync(
                new Entrant(entrantId, 22, "Joe", "Bloggs", "a@a.com", "A", 99, "BRMC", 123456, Domain.Enums.Age.Senior));

            var email = await AuthTools.GetEmail(rd, mediator.Object);

            email.Should().BeEquivalentTo("a@a.com");
            mr.VerifyAll();
        }

        [Fact]
        public async Task GetFromMarshalId()
        {
            var eventId = 1ul;
            var marshalId = 2ul;
            var rd = new RouteData(new RouteValueDictionary());
            rd.Values.Add("eventId", eventId.ToString());
            rd.Values.Add("marshalId", marshalId.ToString());
            mediator.Setup(a => a.Send(Its.EquivalentTo(new GetMarshal(eventId, marshalId)), CancellationToken.None)).ReturnsAsync(
                new Marshal(marshalId, "Joe", "Bloggs", "a@a.com", eventId, 9876543, "role"));

            var email = await AuthTools.GetEmail(rd, mediator.Object);

            email.Should().BeEquivalentTo("a@a.com");
            mr.VerifyAll();
        }
    }
}
