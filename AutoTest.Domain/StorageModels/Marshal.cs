namespace AutoTest.Domain.StorageModels
{
    public class Marshal
    {
        public Marshal(ulong marshalId, string givenName, string familyName, string email, ulong eventId, uint registrationNumber, string role)
        {
            MarshalId = marshalId;
            GivenName = givenName;
            FamilyName = familyName;
            Email = email;
            EventId = eventId;
            RegistrationNumber = registrationNumber;
            Role = role;
        }

        public ulong MarshalId { get; }

        public string GivenName { get; }

        public string FamilyName { get; }


        public ulong EventId { get; }

        public uint RegistrationNumber { get; }
        public string Role { get; }

        public string Email { get; }
        //public AcceptDeclaration AcceptDeclaration { get; private set; } = new AcceptDeclaration();

        public EmergencyContact EmergencyContact { get; private set; } = new EmergencyContact();

        public void SetEmergencyContact(EmergencyContact emergencyContact) => EmergencyContact = emergencyContact;
    }
}
