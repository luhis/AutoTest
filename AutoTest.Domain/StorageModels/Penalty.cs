namespace AutoTest.Domain.StorageModels
{
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
