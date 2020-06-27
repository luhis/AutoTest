using System;

namespace AutoTest.Domain.StorageModels
{
    public class TestRun
    {
        public TestRun(ulong testRunId, ulong testId, ulong timeInMS, ulong entrant, DateTime created)
        {
            TestRunId = testRunId;
            TestId = testId;
            TimeInMS = timeInMS;
            Entrant = entrant;
            Created = created;
        }

        public ulong TestRunId { get; }

        public ulong TestId { get; }

        public ulong TimeInMS { get; }

        public DateTime Created { get; }

        public ulong Entrant { get; }
    }
}
