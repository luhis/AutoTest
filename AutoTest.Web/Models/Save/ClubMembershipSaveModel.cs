using System;
using System.ComponentModel.DataAnnotations;

namespace AutoTest.Web.Models.Save
{
    public class ClubMembershipSaveModel
    {
        [Required]
        public string ClubName { get; set; } = string.Empty;

        [Required]
        public string MembershipNumber { get; set; } = string.Empty;

        [Required]
        public DateOnly Expiry { get; set; }
    }
}
