namespace AutoTest.Domain.StorageModels
{
    public class Profile
    {
        public Profile(string emailAddress, string givenName, string familyName, string msaLicense)
        {
            EmailAddress = emailAddress;
            GivenName = givenName;
            FamilyName = familyName;
            MsaLicense = msaLicense;
        }

        public string EmailAddress { get; }

        public string GivenName { get; }

        public string FamilyName { get; }

        public string MsaLicense { get; }

        public Vehicle Vehicle { get; private set; } = new Vehicle();

        public EmergencyContact EmergencyContact { get; private set; } = new EmergencyContact();

        public void SetVehicle(Vehicle vehicle) => Vehicle = vehicle;

        public void SetEmergencyContact(EmergencyContact emergencyContact) => EmergencyContact = emergencyContact;
    }
}
