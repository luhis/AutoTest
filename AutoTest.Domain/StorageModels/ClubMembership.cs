using System;

namespace AutoTest.Domain.StorageModels
{
    public class ClubMembership(string clubName, string membershipNumber, DateOnly expiry)
    {
        public string ClubName { get; } = clubName;

        public string MembershipNumber { get; } = membershipNumber;

        public DateOnly Expiry { get; } = expiry;
    }
}
