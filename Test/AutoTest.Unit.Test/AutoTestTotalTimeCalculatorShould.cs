using System;
using System.Linq;
using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.ResultCalculation;
using FluentAssertions;
using Xunit;

namespace AutoTest.Unit.Test
{
    public class AutoTestTotalTimeCalculatorShould
    {
        private readonly ITotalTimeCalculator totalTimeCalculator = new AutoTestTotalTimeCalculator();
        private readonly TimeCalculatorConfig _timeCalculatorConfig = new(5_000, 5_000, 5_000, 20_000);

        [Fact]
        public void CalculateSimpleTime()
        {
            var total = totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[] { new TestRun(1, 1, 0, 50_000, 1, DateTime.UtcNow, 1), }, Enumerable.Empty<TestRun>());
            total.Should().Be(50_000);
        }

        [Fact]
        public void TakeShortestTime()
        {
            var total = totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[]
            {
                new TestRun(1, 1, 0, 50_000, 1, DateTime.UtcNow, 1),
                new TestRun(1, 1, 0, 45_000, 1, DateTime.UtcNow, 1),
            }, Enumerable.Empty<TestRun>());
            total.Should().Be(45_000);
        }

        [Fact]
        public void TakeShortestTimeWithCone()
        {
            var runWithCone = new TestRun(1, 1, 0, 44_000, 1, DateTime.UtcNow, 1);
            runWithCone.SetPenalties(new[] { new Penalty(PenaltyEnum.HitBarrier, 1) });
            var total = totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[]
            {
                new TestRun(1, 1, 0, 50_000, 1, DateTime.UtcNow, 1),
                runWithCone,
            }, Enumerable.Empty<TestRun>());
            total.Should().Be(49_000);
        }

        [Fact]
        public void AddTimes()
        {
            var total = totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[]
            {
                new TestRun(1, 1, 0, 45_000, 1, DateTime.UtcNow, 1),
                new TestRun(1, 1, 1, 45_000, 1, DateTime.UtcNow, 1),
            }, Enumerable.Empty<TestRun>());
            total.Should().Be(90_000);
        }

        [Fact]
        public void WrongTestWithNoOthersAttemptsIs20()
        {
            var runWithWrongTest = new TestRun(1, 1, 0, 44_000, 1, DateTime.UtcNow, 1);
            runWithWrongTest.SetPenalties(new[] { new Penalty(PenaltyEnum.WrongTest, 1) });
            var total = totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[]
            {
                runWithWrongTest,
            }, Enumerable.Empty<TestRun>());
            total.Should().Be(20_000);
        }
    }
}
