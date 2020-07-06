using System.Collections.Generic;

namespace AutoTest.Domain.StorageModels
{
    using System;

    public class Event
    {
        public Event(ulong eventId, ulong clubId, string location, DateTime startTime, uint testCount, uint maxAttemptsPerTest)
        {
            EventId = eventId;
            ClubId = clubId;
            Location = location;
            StartTime = startTime;
            TestCount = testCount;
            MaxAttemptsPerTest = maxAttemptsPerTest;
        }

        public ulong EventId { get; }

        public ulong ClubId { get; }

        public string Location { get; }

        public DateTime StartTime { get; }

        public uint TestCount { get; }

        public uint MaxAttemptsPerTest { get; }

        public ICollection<AuthorisationEmail> MarshalEmails { get; private set; } = Array.Empty<AuthorisationEmail>();

        public void SetMarshalEmails(ICollection<AuthorisationEmail> emails) => MarshalEmails = emails;
    }
}
