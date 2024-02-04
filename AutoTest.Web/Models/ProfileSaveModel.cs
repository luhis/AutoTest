using System.Collections.Generic;
using AutoTest.Domain.Enums;

namespace AutoTest.Web.Models
{
    public class ProfileSaveModel
    {
        public string GivenName { get; set; } = string.Empty;

        public string FamilyName { get; set; } = string.Empty;

        public Age Age { get; set; } = Age.Senior;

        public bool IsLady { get; set; }

        public VehicleSaveModel Vehicle { get; set; } = new VehicleSaveModel();

        public EmergencyContactSaveModel EmergencyContact { get; set; } = new EmergencyContactSaveModel();

        public MsaMembershipSaveModel MsaMembership { get; set; } = new MsaMembershipSaveModel();

        public ICollection<ClubMembershipSaveModel> ClubMemberships { get; set; } = new List<ClubMembershipSaveModel>();

    }
}
