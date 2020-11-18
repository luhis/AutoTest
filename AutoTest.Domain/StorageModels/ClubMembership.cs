using System;

namespace AutoTest.Domain.StorageModels
{
    public class ClubMembership
    {
        public ClubMembership(string clubName, string membershipNumber, DateTime expiry)
        {
            ClubName = clubName;
            MembershipNumber = membershipNumber;
            Expiry = expiry;
        }

        public string ClubName { get; }

        public string MembershipNumber { get; }

        public DateTime Expiry { get; }
    }
}
