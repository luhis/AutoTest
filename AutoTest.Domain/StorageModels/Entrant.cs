namespace AutoTest.Domain.StorageModels
{
    public class Entrant
    {

        public Entrant(ulong entrantId, int driverNumber, string givenName, string familyName, string @class, ulong eventId, bool isPaid)
        {
            EntrantId = entrantId;
            GivenName = givenName;
            FamilyName = familyName;
            Class = @class;
            EventId = eventId;
            IsPaid = isPaid;
            DriverNumber = driverNumber;
        }

        public ulong EntrantId { get; }

        public int DriverNumber { get; }

        public string GivenName { get; }

        public string FamilyName { get; }

        public string Class { get; }

        public ulong EventId { get; }

        public bool IsPaid { get; }

        public Vehicle Vehicle { get; private set; } = new Vehicle();

        public EmergencyContact EmergencyContact { get; private set; } = new EmergencyContact();

        public void SetVehicle(Vehicle vehicle) => Vehicle = vehicle;
        public void SetEmergencyContact(EmergencyContact emergencyContact) => EmergencyContact = emergencyContact;
    }
}
