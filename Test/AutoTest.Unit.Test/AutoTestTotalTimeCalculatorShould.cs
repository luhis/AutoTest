﻿using System;
using System.Linq;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.ResultCalculation;
using FluentAssertions;
using Xunit;

namespace AutoTest.Unit.Test
{
    public class AutoTestTotalTimeCalculatorShould
    {
        private readonly ITotalTimeCalculator totalTimeCalculator = new AutoTestTotalTimeCalculator();

        [Fact]
        public void CalculateSimpleTime()
        {
            var total = totalTimeCalculator.GetTotalTime(new[] { new TestRun(1, 1, 0, 50_000, 1, DateTime.UtcNow), }, Enumerable.Empty<TestRun>());
            total.Should().Be(50_000);
        }

        [Fact]
        public void TakeShortestTime()
        {
            var total = totalTimeCalculator.GetTotalTime(new[]
            {
                new TestRun(1, 1, 0, 50_000, 1, DateTime.UtcNow),
                new TestRun(1, 1, 0, 45_000, 1, DateTime.UtcNow),
            }, Enumerable.Empty<TestRun>());
            total.Should().Be(45_000);
        }

        [Fact]
        public void TakeShortestTimeWithCode()
        {
            var runWithCone = new TestRun(1, 1, 0, 44_000, 1, DateTime.UtcNow);
            runWithCone.SetPenalties(new[] { new Penalty(PenaltyEnum.HitBarrier, 1) });
            var total = totalTimeCalculator.GetTotalTime(new[]
            {
                new TestRun(1, 1, 0, 50_000, 1, DateTime.UtcNow),
                runWithCone,
            }, Enumerable.Empty<TestRun>());
            total.Should().Be(49_000);
        }

        [Fact]
        public void AddTimes()
        {
            var total = totalTimeCalculator.GetTotalTime(new[]
            {
                new TestRun(1, 1, 0, 45_000, 1, DateTime.UtcNow),
                new TestRun(1, 1, 1, 45_000, 1, DateTime.UtcNow),
            }, Enumerable.Empty<TestRun>());
            total.Should().Be(90_000);
        }

        [Fact]
        public void WrongTestWithNoOthersAttemptsIs20()
        {
            var runWithWrongTest = new TestRun(1, 1, 0, 44_000, 1, DateTime.UtcNow);
            runWithWrongTest.SetPenalties(new[] { new Penalty(PenaltyEnum.WrongTest, 1) });
            var total = totalTimeCalculator.GetTotalTime(new[]
            {
                runWithWrongTest,
            }, Enumerable.Empty<TestRun>());
            total.Should().Be(20_000);
        }
    }
}
