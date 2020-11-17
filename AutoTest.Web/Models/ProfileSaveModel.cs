namespace AutoTest.Web.Models
{
    public class ProfileSaveModel
    {
        public string EmailAddress { get; set; } = string.Empty;

        public string GivenName { get; set; } = string.Empty;

        public string FamilyName { get; set; } = string.Empty;

        public string MsaLicense { get; set; } = string.Empty;

        public VehicleSaveModel Vehicle { get; set; } = new VehicleSaveModel();

        public EmergencyContactSaveModel EmergencyContact { get; set; } = new EmergencyContactSaveModel();

    }
}
