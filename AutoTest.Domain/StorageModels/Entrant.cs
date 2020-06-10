namespace AutoTest.Domain.StorageModels
{
    public class Entrant
    {
        public Entrant(ulong entrantId, string registration, string category, ulong eventId, bool isPaid)
        {
            EntrantId = entrantId;
            Registration = registration;
            Category = category;
            this.EventId = eventId;
            this.IsPaid = isPaid;
        }

        public ulong EntrantId { get; }

        public string Registration { get; }

        public string Category { get; }

        public ulong EventId { get; }

        public bool IsPaid { get; }
    }
}