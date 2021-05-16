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

        public bool IsPaid { get; set; }

        public string Email { get; set; } = string.Empty;

        public VehicleSaveModel Vehicle { get; set; } = new VehicleSaveModel();

        public MsaMembershipSaveModel MsaMembership { get; set; } = new MsaMembershipSaveModel();

        public EmergencyContactSaveModel EmergencyContact { get; set; } = new EmergencyContactSaveModel();
    }
}
