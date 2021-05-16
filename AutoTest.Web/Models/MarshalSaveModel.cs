namespace AutoTest.Web.Models
{
    public class MarshalSaveModel
    {

        public string GivenName { get; set; } = string.Empty;

        public string FamilyName { get; set; } = string.Empty;

        public uint RegistrationNumber { get; set; }

        public string Role { get; set; } = string.Empty;

        public EmergencyContactSaveModel EmergencyContact { get; set; } = new EmergencyContactSaveModel();
    }
}
