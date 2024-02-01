using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
using FluentAssertions.ArgumentMatchers.Moq;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class GetTestsHandlerShould
    {
        private readonly IRequestHandler<GetTests, IEnumerable<Domain.StorageModels.Course>> sut;
        private readonly MockRepository mr;
        private readonly Mock<IEventsRepository> testRuns;

        public GetTestsHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            testRuns = mr.Create<IEventsRepository>();
            sut = new GetTestsHandler(testRuns.Object);
        }

        [Fact]
        public async Task ShouldNotifyOnUpdatedTestRun()
        {
            var entrantId = 5ul;
            var marshalId = 6ul;
            var penalties = new[] { new Penalty(Domain.Enums.PenaltyEnum.Late, 1) };
            var tr = new TestRun(1, 2, 3, 4, entrantId, new System.DateTime(2000, 1, 1), marshalId);
            tr.SetPenalties(penalties);
            testRuns.Setup(a => a.GetById(1, CancellationToken.None)).ReturnsAsync(Models.GetEvent(1));

            await sut.Handle(new(1), CancellationToken.None);
            // todo check result
            mr.VerifyAll();
        }
    }
}
