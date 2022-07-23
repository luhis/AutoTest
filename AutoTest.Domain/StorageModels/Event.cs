using System.Collections.Generic;
using System;
using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels
{

    public class Event
    {
        public Event(ulong eventId, ulong clubId, string location, DateTime startTime, int testCount, int maxAttemptsPerTest, string regulations, EventType eventType, string maps, TimingSystem timingSystem, DateTime entryOpenDate, DateTime entryCloseDate)
        {
            EventId = eventId;
            ClubId = clubId;
            Location = location;
            StartTime = startTime;
            TestCount = testCount;
            MaxAttemptsPerTest = maxAttemptsPerTest;
            Regulations = regulations;
            EventType = eventType;
            Maps = maps;
            TimingSystem = timingSystem;
            EntryOpenDate = entryOpenDate;
            EntryCloseDate = entryCloseDate;
        }

        public ulong EventId { get; }

        public ulong ClubId { get; }

        public string Location { get; }

        public DateTime StartTime { get; }

        public int TestCount { get; }

        public int MaxAttemptsPerTest { get; }

        public ICollection<Test> Tests { get; private set; } = new List<Test>();

        public void SetTests(ICollection<Test> tests) => Tests = tests;

        public string Regulations { get; }

        public string Maps { get; }

        public EventStatus EventStatus { get; private set; }

        public EventType EventType { get; }

        public TimingSystem TimingSystem { get; }

        public DateTime EntryOpenDate { get; }

        public DateTime EntryCloseDate { get; }

        public void SetEventStats(EventStatus eventStatus) => this.EventStatus = EventStatus;

    }
}
