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

        public string MsaLicense { get; set; } = string.Empty;

        public bool IsPaid { get; set; }

        public VehicleSaveModel Vehicle { get; set; } = new VehicleSaveModel();

        public EmergencyContactSaveModel EmergencyContact { get; set; } = new EmergencyContactSaveModel();
    }
}
