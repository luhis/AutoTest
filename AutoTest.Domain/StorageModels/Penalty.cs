namespace AutoTest.Domain.StorageModels
{
    public class Penalty
    {
        public Penalty(ulong testRunId, PenaltyEnum penaltyType, uint instanceCount)
        {
            PenaltyType = penaltyType;
            InstanceCount = instanceCount;
            TestRunId = testRunId;
        }

        public PenaltyEnum PenaltyType { get; }

        public uint InstanceCount { get; }

        public ulong TestRunId { get; }
    }
}
