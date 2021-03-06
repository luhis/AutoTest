﻿using System.Collections.Generic;
using System;
using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels
{

    public class Event
    {
        public Event(ulong eventId, ulong clubId, string location, DateTime startTime, int testCount, int maxAttemptsPerTest, string regulations, EventType eventType, string maps, TimingSystem timingSystem)
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

        public EventType EventType { get; }

        public TimingSystem TimingSystem { get; }

    }
}
