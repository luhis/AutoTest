using System;
using System.Linq;
using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.ResultCalculation;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Unit.Test;

public class AutoTestTotalTimeCalculatorShould
{
    private readonly ITotalTimeCalculator _totalTimeCalculator = new AutoTestTotalTimeCalculator();
    private readonly TimeCalculatorConfig _timeCalculatorConfig = new(5_000, 5_000, 5_000, 20_000);

    [Fact]
    public void CalculateSimpleTime()
    {
        var total = _totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[] { new TestRun(1, 1, 0, 50_000, 1, DateTime.UtcNow, 1), }, Enumerable.Empty<TestRun>());
        total.Should().Be(50_000);
    }

    [Fact]
    public void TakeShortestTime()
    {
        var total = _totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[]
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
        var total = _totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[]
        {
            new TestRun(1, 1, 0, 50_000, 1, DateTime.UtcNow, 1),
            runWithCone,
        }, Enumerable.Empty<TestRun>());
        total.Should().Be(49_000);
    }

    [Fact]
    public void AddTimes()
    {
        var total = _totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[]
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
        var total = _totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[]
        {
            runWithWrongTest,
        }, Enumerable.Empty<TestRun>());
        total.Should().Be(20_000);
    }

    [Fact]
    public void AddFailToStopPenalty()
    {
        var total = _totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[]
        {
            new TestRun(1, 1, 0, 50_000, 1, DateTime.UtcNow, 1),
        }, Enumerable.Empty<TestRun>());
        var runWithPenalty = new TestRun(1, 1, 0, 50_000, 1, DateTime.UtcNow, 1);
        runWithPenalty.SetPenalties(new[] { new Penalty(PenaltyEnum.FailToStop, 1) });
        var totalWithPenalty = _totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[]
        {
            runWithPenalty,
        }, Enumerable.Empty<TestRun>());
        totalWithPenalty.Should().Be(total + 5_000);
    }

    [Fact]
    public void AddLatePenalty()
    {
        var runWithLate = new TestRun(1, 1, 0, 50_000, 1, DateTime.UtcNow, 1);
        runWithLate.SetPenalties(new[] { new Penalty(PenaltyEnum.Late, 1) });
        var total = _totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[]
        {
            runWithLate,
        }, Enumerable.Empty<TestRun>());
        total.Should().Be(55_000);
    }

    [Fact]
    public void AddMultipleLatePenalties()
    {
        var runWithLate = new TestRun(1, 1, 0, 50_000, 1, DateTime.UtcNow, 1);
        runWithLate.SetPenalties(new[] { new Penalty(PenaltyEnum.Late, 3) });
        var total = _totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[]
        {
            runWithLate,
        }, Enumerable.Empty<TestRun>());
        total.Should().Be(65_000);
    }

    [Fact]
    public void CombineMultiplePenalties()
    {
        var run = new TestRun(1, 1, 0, 50_000, 1, DateTime.UtcNow, 1);
        run.SetPenalties(new[]
        {
            new Penalty(PenaltyEnum.HitBarrier, 1),
            new Penalty(PenaltyEnum.FailToStop, 1),
            new Penalty(PenaltyEnum.Late, 1),
        });
        var total = _totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[]
        {
            run,
        }, Enumerable.Empty<TestRun>());
        total.Should().Be(65_000);
    }

    [Fact]
    public void NoAttendanceUsesFastestCorrectRunPlusNoTest()
    {
        var correctRun = new TestRun(1, 1, 0, 45_000, 1, DateTime.UtcNow, 1);
        var noAttendanceRun = new TestRun(1, 1, 0, 50_000, 2, DateTime.UtcNow, 1);
        noAttendanceRun.SetPenalties(new[] { new Penalty(PenaltyEnum.NoAttendance, 1) });
        var total = _totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[]
        {
            noAttendanceRun,
        }, new[] { correctRun, noAttendanceRun });
        total.Should().Be(65_000);
    }

    [Fact]
    public void EmptyTestRunsReturnsZero()
    {
        var total = _totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, Enumerable.Empty<TestRun>(), Enumerable.Empty<TestRun>());
        total.Should().Be(0);
    }

    [Fact]
    public void AllWrongTestsWithNoCorrectRunsIsNoTest()
    {
        var wrongRun1 = new TestRun(1, 1, 0, 44_000, 1, DateTime.UtcNow, 1);
        wrongRun1.SetPenalties(new[] { new Penalty(PenaltyEnum.WrongTest, 1) });
        var wrongRun2 = new TestRun(1, 1, 0, 46_000, 2, DateTime.UtcNow, 1);
        wrongRun2.SetPenalties(new[] { new Penalty(PenaltyEnum.WrongTest, 1) });
        var total = _totalTimeCalculator.GetTotalTime(_timeCalculatorConfig, new[]
        {
            wrongRun1,
            wrongRun2,
        }, new[] { wrongRun1, wrongRun2 });
        total.Should().Be(20_000);
    }
}
