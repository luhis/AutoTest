using System;

namespace AutoTest.Domain.StorageModels
{
    public class ClubMembership
    {
        public ClubMembership(string clubName, string membershipNumber, DateOnly expiry)
        {
            ClubName = clubName;
            MembershipNumber = membershipNumber;
            Expiry = expiry;
        }

        public string ClubName { get; }

        public string MembershipNumber { get; }

        public DateOnly Expiry { get; }
    }
}
