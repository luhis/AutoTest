using System;

namespace AutoTest.Web.Models
{
    public class ClubMembershipSaveModel
    {
        public string ClubName { get; set; } = string.Empty;

        public string MembershipNumber { get; set; } = string.Empty;

        public DateOnly Expiry { get; set; }
    }
}
