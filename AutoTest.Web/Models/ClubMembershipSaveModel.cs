using System;

namespace AutoTest.Web.Models
{
    public class ClubMembershipSaveModel
    {
        public string ClubName { get; set; } = string.Empty;

        public uint MembershipNumber { get; set; }

        public DateTime Expiry { get; set; }
    }
}
