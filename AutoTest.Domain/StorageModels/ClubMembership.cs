using System;

namespace AutoTest.Domain.StorageModels;

public record ClubMembership(string ClubName, string MembershipNumber, DateOnly Expiry);
