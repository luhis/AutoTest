namespace AutoTest.Domain.StorageModels
{
    public class Entrant
    {

        public Entrant(ulong entrantId, string givenName, string familyName, string @class, ulong eventId, bool isPaid, Vehicle vehicle)
        {
            GivenName = givenName;
            FamilyName = familyName;
            EntrantId = entrantId;
            Class = @class;
            EventId = eventId;
            IsPaid = isPaid;
            Vehicle = vehicle;
        }

        public ulong EntrantId { get; }

        public string GivenName { get; }

        public string FamilyName { get; }

        public string Class { get; }

        public ulong EventId { get; }

        public bool IsPaid { get; }

        public Vehicle Vehicle { get; }
    }
}
