using System;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<Penalty> Penalties { get; private set; } = Enumerable.Empty<Penalty>();
    }

    public class Penalty
    {
        public Penalty(ulong penaltyId, PenaltyEnum penaltyType, ulong testRunId, uint instanceCount)
        {
            PenaltyId = penaltyId;
            PenaltyType = penaltyType;
            TestRunId = testRunId;
            InstanceCount = instanceCount;
        }

        public ulong PenaltyId { get; }

        public PenaltyEnum PenaltyType { get; }

        public ulong TestRunId { get; }

        public uint InstanceCount { get; set; }
    }
}
