using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Service.Models;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class GetResultsHandlerShould
    {
        private readonly IRequestHandler<GetResults, IEnumerable<Result>> sut;
        private readonly MockRepository mr;
        private readonly Mock<ITestRunsRepository> testRunsRepository;
        private readonly Mock<IEventsRepository> eventsRepository;
        private readonly Mock<IEntrantsRepository> entrantsRepository;

        public GetResultsHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            testRunsRepository = mr.Create<ITestRunsRepository>();
            eventsRepository = mr.Create<IEventsRepository>();
            entrantsRepository = mr.Create<IEntrantsRepository>();
            sut = new GetResultsHandler(testRunsRepository.Object, eventsRepository.Object, entrantsRepository.Object);
        }

        [Fact]
        public async Task GetResults()
        {
            var entrantId = 1ul;
            var eventId = 22ul;

            eventsRepository.Setup(a => a.GetById(eventId, CancellationToken.None)).ReturnsAsync(
                new Event(eventId, 1, "location", new System.DateTime(2000, 1, 1), 2, 3, "regs", new[] { EventType.AutoSolo }, "maps", TimingSystem.App, new DateTime(), new DateTime(), 10)
                );
            var entrant = new Entrant(entrantId, 1, "matt", "mccorry", "a@a.com", "A", eventId, "BRMC", 1234, Age.Senior);
            entrant.SetPayment(new(new System.DateTime(2000, 1, 1), Domain.Enums.PaymentMethod.Paypal, new System.DateTime(2000, 2, 2)));
            entrantsRepository.Setup(a => a.GetByEventId(eventId, CancellationToken.None)).ReturnsAsync(new[] { entrant });
            testRunsRepository.Setup(a => a.GetAll(eventId, CancellationToken.None)).ReturnsAsync(Enumerable.Empty<TestRun>());

            await sut.Handle(new(eventId), CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
