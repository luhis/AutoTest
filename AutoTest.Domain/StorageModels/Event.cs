using System.Collections.Generic;
using System.Linq;

namespace AutoTest.Domain.StorageModels
{
    using System;

    public class Event
    {
        public Event(ulong eventId, ulong clubId, string location, DateTime startTime, int testCount, int maxAttemptsPerTest)
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

        public int TestCount { get; }

        public int MaxAttemptsPerTest { get; }

        public ICollection<AuthorisationEmail> MarshalEmails { get; private set; } = new List<AuthorisationEmail>();

        public void SetMarshalEmails(ICollection<AuthorisationEmail> emails) => MarshalEmails = emails;

        public ICollection<Test> Tests { get; private set; } = new List<Test>();

        public void SetTests(ICollection<Test> tests) => Tests = tests;
    }
}
