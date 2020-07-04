namespace AutoTest.Domain.StorageModels
{
    public class Penalty
    {
        public Penalty(PenaltyEnum penaltyType, uint instanceCount)
        {
            PenaltyType = penaltyType;
            InstanceCount = instanceCount;
        }

        public PenaltyEnum PenaltyType { get; }

        public uint InstanceCount { get; }
    }
}
