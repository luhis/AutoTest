namespace AutoTest.Domain.StorageModels
{
    public class Penalty
    {
        public Penalty(PenaltyEnum penaltyType, int instanceCount)
        {
            PenaltyType = penaltyType;
            InstanceCount = instanceCount;
        }

        public PenaltyEnum PenaltyType { get; }

        public int InstanceCount { get; }
    }
}
