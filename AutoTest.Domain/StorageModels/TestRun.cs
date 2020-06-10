namespace AutoTest.Domain.StorageModels
{
    public class TestRun
    {
        public TestRun(ulong testRunId, ulong testId, ulong timeInMS, ulong entrant)
        {
            TestRunId = testRunId;
            TestId = testId;
            TimeInMS = timeInMS;
            this.Entrant = entrant;
        }

        public ulong TestRunId { get; }

        public ulong TestId { get; }

        public ulong TimeInMS { get; }

        public ulong Entrant { get; }
    }
}