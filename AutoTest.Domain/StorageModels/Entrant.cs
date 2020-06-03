namespace AutoTest.Domain.StorageModels
{
    public class Entrant
    {
        public Entrant(ulong entrantId, string registration, string category)
        {
            EntrantId = entrantId;
            Registration = registration;
            Category = category;
        }

        public ulong EntrantId { get; }

        public string Registration { get; }

        public string Category { get; }
    }
}