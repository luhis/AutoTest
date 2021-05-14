using System;

namespace AutoTest.Domain.StorageModels
{
    public class ClubMembership
    {
        public ClubMembership(string clubName, uint membershipNumber, DateTime expiry)
        {
            ClubName = clubName;
            MembershipNumber = membershipNumber;
            Expiry = expiry;
        }

        public string ClubName { get; }

        public uint MembershipNumber { get; }

        public DateTime Expiry { get; }
    }
}
