using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels;

public record Penalty(PenaltyEnum PenaltyType, int InstanceCount);
