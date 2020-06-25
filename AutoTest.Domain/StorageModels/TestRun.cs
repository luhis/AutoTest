using System;

namespace AutoTest.Domain.StorageModels
{
    public class TestRun
    {
        public TestRun(ulong testRunId, ulong testId, ulong timeInMS, ulong entrantId, DateTime created)
        {
            TestRunId = testRunId;
            TestId = testId;
            TimeInMS = timeInMS;
            EntrantId = entrantId;
            Created = created;
        }

        public ulong TestRunId { get; }

        public ulong TestId { get; }

        public ulong TimeInMS { get; }

        public DateTime Created { get; }

        public ulong EntrantId { get; }
    }
}
