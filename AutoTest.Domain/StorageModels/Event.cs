namespace AutoTest.Domain.StorageModels
{
    using System;

    public class Event
    {
        public Event(ulong eventId, ulong clubId, string location, DateTime startTime, uint testCount)
        {
            EventId = eventId;
            ClubId = clubId;
            Location = location;
            StartTime = startTime;
            TestCount = testCount;
        }

        public ulong EventId { get; }

        public ulong ClubId { get; }

        public string Location { get; }

        public DateTime StartTime { get; }

        public uint TestCount { get; }
    }
}
