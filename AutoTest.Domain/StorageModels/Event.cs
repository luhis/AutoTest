namespace AutoTest.Domain.StorageModels
{
    using System;

    public class Event
    {
        public Event(ulong eventId, string location, DateTime startTime)
        {
            EventId = eventId;
            Location = location;
            StartTime = startTime;
        }

        public ulong EventId { get; }

        public string Location { get; }

        public DateTime StartTime { get; }
    }
}