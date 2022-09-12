using System.Collections.Generic;
using System;
using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels
{

    public class Event
    {
        public Event(ulong eventId, ulong clubId, string location, DateTime startTime, int testCount, int maxAttemptsPerTest, string regulations, ICollection<EventType> eventTypes, string maps, TimingSystem timingSystem, DateTime entryOpenDate, DateTime entryCloseDate, uint maxEntrants)
        {
            EventId = eventId;
            ClubId = clubId;
            Location = location;
            StartTime = startTime;
            TestCount = testCount;
            MaxAttemptsPerTest = maxAttemptsPerTest;
            Regulations = regulations;
            EventTypes = eventTypes;
            Maps = maps;
            TimingSystem = timingSystem;
            EntryOpenDate = entryOpenDate;
            EntryCloseDate = entryCloseDate;
            MaxEntrants = maxEntrants;
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

        public ICollection<EventType> EventTypes { get; private set; } = new List<EventType>();
        public void SetEventTypes(ICollection<EventType> eventTypes) => EventTypes = eventTypes;

        public TimingSystem TimingSystem { get; }

        public DateTime EntryOpenDate { get; }

        public DateTime EntryCloseDate { get; }

        public uint MaxEntrants { get; }

        public void SetEventStatus(EventStatus eventStatus) => this.EventStatus = eventStatus;

    }
}
