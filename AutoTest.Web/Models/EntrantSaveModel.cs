using AutoTest.Domain.Enums;

namespace AutoTest.Web.Models
{
    public class EntrantSaveModel
    {
        public ushort DriverNumber { get; set; }

        public string GivenName { get; set; } = string.Empty;

        public string FamilyName { get; set; } = string.Empty;

        public string Class { get; set; } = string.Empty;

        public string Club { get; set; } = string.Empty;

        public uint ClubNumber { get; set; }

        public string Email { get; set; } = string.Empty;

        public Age Age { get; set; } = Age.Senior;

        public VehicleSaveModel Vehicle { get; set; } = new VehicleSaveModel();

        public MsaMembershipSaveModel MsaMembership { get; set; } = new MsaMembershipSaveModel();

        public EmergencyContactSaveModel EmergencyContact { get; set; } = new EmergencyContactSaveModel();

        public AcceptDeclarationSaveModel AcceptDeclaration { get; set; } = new AcceptDeclarationSaveModel();
        public EventType EventType { get; internal set; }

        public bool IsLady { get; set; }
    }
}
