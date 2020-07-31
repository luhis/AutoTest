using System.Collections.Generic;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Service.Models
{
    public class TestTime
    {
        public TestTime(int ordinal, IEnumerable<TestRun> testRuns)
        {
            Ordinal = ordinal;
            TestRuns = testRuns;
        }

        public int Ordinal { get; }

        public IEnumerable<TestRun> TestRuns { get; }
    }
}
