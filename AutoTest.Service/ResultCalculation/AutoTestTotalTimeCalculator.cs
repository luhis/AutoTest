using System.Collections.Generic;
using System.Linq;
using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Service.ResultCalculation
{
    public class AutoTestTotalTimeCalculator : ITotalTimeCalculator
    {
        int ITotalTimeCalculator.GetTotalTime(TimeCalculatorConfig config, IEnumerable<TestRun> testRuns, IEnumerable<TestRun> allTestRuns) =>
            testRuns.GroupBy(a => a.Ordinal).Select(a => GetTime(config, a, allTestRuns.Where(b => b.Ordinal == a.Key))).Sum();

        private static int GetTime(TimeCalculatorConfig config, IEnumerable<TestRun> testRuns, IEnumerable<TestRun> allTestRuns) => testRuns.Select(a => GetFinalTime(config, a, allTestRuns)).Min();

        private static int GetFinalTime(TimeCalculatorConfig config, TestRun testRun, IEnumerable<TestRun> allTestRuns)
        {
            if (IsInCorrectRun(testRun))
            {
                var fastest = GetFastestCorrectRun(config, allTestRuns);
                return (fastest?.TimeInMS ?? 0) + config.NoTest;
            }

            return testRun.TimeInMS + GetInstanceCount(testRun.Penalties, PenaltyEnum.FailToStop) * config.FailStop + GetInstanceCount(testRun.Penalties, PenaltyEnum.HitBarrier) * config.Barrier + GetInstanceCount(testRun.Penalties, PenaltyEnum.Late) * config.Late;
        }

        private static int GetInstanceCount(IEnumerable<Penalty> penalties, PenaltyEnum type) =>
            penalties.Where(a => a.PenaltyType == type).Select(a => a.InstanceCount).Sum();

        private static TestRun? GetFastestCorrectRun(TimeCalculatorConfig config, IEnumerable<TestRun> allTestRuns) => allTestRuns.Where(a => !IsInCorrectRun(a)).OrderBy(a => GetFinalTime(config, a, Enumerable.Empty<TestRun>())).FirstOrDefault();

        private static readonly IReadOnlySet<PenaltyEnum> InCorrectTypes = new HashSet<PenaltyEnum>() { PenaltyEnum.WrongTest, PenaltyEnum.NoAttendance };
        private static bool IsInCorrectRun(TestRun tr) => tr.Penalties.Any(a =>
            InCorrectTypes.Contains(a.PenaltyType) &&
            a.InstanceCount > 0);
    }
}
