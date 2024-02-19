using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoTest.Domain.Enums;

namespace AutoTest.Web.Models
{
    public class ProfileSaveModel
    {
        [Required]
        public string GivenName { get; set; } = string.Empty;

        [Required]
        public string FamilyName { get; set; } = string.Empty;

        [Required]
        public Age Age { get; set; } = Age.Senior;

        [Required]
        public bool IsLady { get; set; }

        [Required]
        public VehicleSaveModel Vehicle { get; set; } = new VehicleSaveModel();

        [Required]
        public EmergencyContactSaveModel EmergencyContact { get; set; } = new EmergencyContactSaveModel();

        [Required]
        public MsaMembershipSaveModel MsaMembership { get; set; } = new MsaMembershipSaveModel();

        [Required]
        public ICollection<ClubMembershipSaveModel> ClubMemberships { get; set; } = new List<ClubMembershipSaveModel>();

    }
}
