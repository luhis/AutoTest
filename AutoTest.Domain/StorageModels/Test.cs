namespace AutoTest.Domain.StorageModels
{
    public class Test
    {
        public Test(ulong testId, ulong eventId, int ordinal, string? mapLocation)
        {
            TestId = testId;
            EventId = eventId;
            Ordinal = ordinal;
            this.MapLocation = mapLocation;
        }

        public ulong TestId { get; }

        public ulong EventId { get; }

        public int Ordinal { get; }

        public string? MapLocation { get; }
    }
}
