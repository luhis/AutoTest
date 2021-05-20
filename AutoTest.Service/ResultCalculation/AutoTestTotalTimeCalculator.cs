using System.Collections.Generic;
using System.Linq;
using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Service.ResultCalculation
{
    public class AutoTestTotalTimeCalculator : ITotalTimeCalculator
    {
        int ITotalTimeCalculator.GetTotalTime(IEnumerable<TestRun> testRuns, IEnumerable<TestRun> allTestRuns)
        {
            return testRuns.GroupBy(a => a.Ordinal).Select(a => GetTime(a, allTestRuns.Where(b => b.Ordinal == a.Key))).Sum();
        }

        private const int FiveSecs = 5_000;

        private static int GetTime(IEnumerable<TestRun> testRuns, IEnumerable<TestRun> allTestRuns) => testRuns.Select(a => GetFinalTime(a, allTestRuns)).Min();

        private static int GetFinalTime(TestRun testRun, IEnumerable<TestRun> allTestRuns)
        {
            if (IsInCorrectRun(testRun))
            {
                var fastest = GetFastestCorrectRun(allTestRuns);
                return (fastest?.TimeInMS ?? 0) + 20_000;
            }

            return testRun.TimeInMS + GetInstanceCount(testRun.Penalties, PenaltyEnum.FailToStop) * FiveSecs + GetInstanceCount(testRun.Penalties, PenaltyEnum.HitBarrier) * FiveSecs + GetInstanceCount(testRun.Penalties, PenaltyEnum.Late) * FiveSecs;
        }

        private static int GetInstanceCount(IEnumerable<Penalty> penalties, PenaltyEnum type) =>
            penalties.Where(a => a.PenaltyType == type).Select(a => a.InstanceCount).Sum();

        private static TestRun? GetFastestCorrectRun(IEnumerable<TestRun> allTestRuns) => allTestRuns.Where(a => !IsInCorrectRun(a)).OrderBy(a => GetFinalTime(a, Enumerable.Empty<TestRun>())).FirstOrDefault();

        private static bool IsInCorrectRun(TestRun tr) => tr.Penalties.Any(a =>
            (a.PenaltyType == PenaltyEnum.WrongTest || a.PenaltyType == PenaltyEnum.NoAttendance) &&
            a.InstanceCount > 0);
    }
}
