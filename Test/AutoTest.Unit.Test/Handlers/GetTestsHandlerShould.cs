using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class GetTestsHandlerShould
    {
        private readonly IRequestHandler<GetTests, IEnumerable<Domain.StorageModels.Course>> sut;
        private readonly MockRepository mr;
        private readonly Mock<IEventsRepository> eventsRepository;

        public GetTestsHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            eventsRepository = mr.Create<IEventsRepository>();
            sut = new GetTestsHandler(eventsRepository.Object);
        }

        [Fact]
        public async Task GetTests()
        {
            var penalties = new[] { new Penalty(Domain.Enums.PenaltyEnum.Late, 1) };
            var @event = Models.GetEvent(1);
            @event.SetCourses(new[] { new Course(0, "a") });
            eventsRepository.Setup(a => a.GetById(1, CancellationToken.None)).ReturnsAsync(@event);

            var tests = await sut.Handle(new(1), CancellationToken.None);

            tests.Should().BeEquivalentTo(new[] { new Course(0, "a") });
            mr.VerifyAll();
        }
    }
}
