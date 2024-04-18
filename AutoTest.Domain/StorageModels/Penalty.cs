using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels
{
    public class Penalty(PenaltyEnum penaltyType, int instanceCount)
    {
        public PenaltyEnum PenaltyType { get; } = penaltyType;

        public int InstanceCount { get; } = instanceCount;
    }
}
