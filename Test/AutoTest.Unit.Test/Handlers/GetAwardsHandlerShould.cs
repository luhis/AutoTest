using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Service.Models;
using AutoTest.Unit.Test.MockData;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class GetAwardsHandlerShould
    {
        private readonly IRequestHandler<GetAwards, Awards> sut;
        private readonly MockRepository mr;
        private readonly Mock<ITestRunsRepository> testRunsRepository;
        private readonly Mock<IEventsRepository> eventsRepository;
        private readonly Mock<IEntrantsRepository> entrantsRepository;

        public GetAwardsHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            testRunsRepository = mr.Create<ITestRunsRepository>();
            eventsRepository = mr.Create<IEventsRepository>();
            entrantsRepository = mr.Create<IEntrantsRepository>();
            sut = new GetAwardsHandler(testRunsRepository.Object, eventsRepository.Object, entrantsRepository.Object);
        }

        [Fact]
        public async Task GetAwards()
        {
            var entrantId = 1ul;
            var eventId = 22ul;

            eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(
                Models.GetEvent(eventId)
                );
            var entrant = Models.GetEntrant(entrantId, eventId);
            var entrant2 = Models.GetEntrant(entrantId + 1, eventId);
            entrantsRepository.Setup(a => a.GetByEventId(eventId, CancellationToken.None)).ReturnsAsync(new[] { entrant, entrant2 });
            testRunsRepository.Setup(a => a.GetAll(eventId, CancellationToken.None)).ReturnsAsync(Enumerable.Empty<TestRun>());

            var res = await sut.Handle(new(eventId), CancellationToken.None);

            res.Should().BeEquivalentTo(new Awards(new EntrantTimes(entrant, 0, Enumerable.Empty<TestTime>(), 0, 0), new[] {
                new Result("A", new[] { new EntrantTimes(entrant2, 0, Enumerable.Empty<TestTime>(), 1, 0) })
            }));

            mr.VerifyAll();
        }
    }
}
