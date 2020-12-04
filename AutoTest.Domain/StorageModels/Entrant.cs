namespace AutoTest.Domain.StorageModels
{
    public class Entrant
    {

        public Entrant(ulong entrantId, ushort driverNumber, string givenName, string familyName, string @class, ulong eventId, bool isPaid, string club, string msaLicense)
        {
            EntrantId = entrantId;
            GivenName = givenName;
            FamilyName = familyName;
            Class = @class;
            EventId = eventId;
            IsPaid = isPaid;
            Club = club;
            MsaLicense = msaLicense;
            DriverNumber = driverNumber;
        }

        public ulong EntrantId { get; }

        public ushort DriverNumber { get; }

        public string GivenName { get; }

        public string FamilyName { get; }

        public string Class { get; }

        public ulong EventId { get; }

        public bool IsPaid { get; }

        public string Club { get; }
        public string MsaLicense { get; }

        public Vehicle Vehicle { get; private set; } = new Vehicle();

        public EmergencyContact EmergencyContact { get; private set; } = new EmergencyContact();

        public void SetVehicle(Vehicle vehicle) => Vehicle = vehicle;

        public void SetEmergencyContact(EmergencyContact emergencyContact) => EmergencyContact = emergencyContact;
    }
}
