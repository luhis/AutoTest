namespace AutoTest.Domain.StorageModels
{
    public class Entrant
    {

        public Entrant(ulong entrantId, string givenName, string familyName, string @class, ulong eventId, bool isPaid)
        {
            GivenName = givenName;
            FamilyName = familyName;
            EntrantId = entrantId;
            Class = @class;
            EventId = eventId;
            IsPaid = isPaid;
        }

        public ulong EntrantId { get; }

        public string GivenName { get; }

        public string FamilyName { get; }

        public string Class { get; }

        public ulong EventId { get; }

        public bool IsPaid { get; }

        public Vehicle Vehicle { get; private set; } = new Vehicle();

        public EmergencyContact EmergencyContact { get; private set; } = new EmergencyContact();

        public void SetVehicle(Vehicle vehicle) => Vehicle = vehicle;
    }
}
