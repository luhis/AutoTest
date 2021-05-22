using System;
using System.Collections.Generic;

namespace AutoTest.Domain.StorageModels
{
    public class TestRun
    {
        public TestRun(ulong testRunId, ulong eventId, int ordinal, int timeInMS, ulong entrantId, DateTime created, ulong marshalId)
        {
            TestRunId = testRunId;
            EventId = eventId;
            Ordinal = ordinal;
            TimeInMS = timeInMS;
            EntrantId = entrantId;
            Created = created;
            MarshalId = marshalId;
        }

        public int Ordinal { get; set; }

        public ulong TestRunId { get; }

        public ulong EventId { get; }

        public int TimeInMS { get; }

        public DateTime Created { get; }

        public ulong EntrantId { get; }

        public ulong MarshalId { get; }

        public ICollection<Penalty> Penalties { get; private set; } = new List<Penalty>();

        public void SetPenalties(ICollection<Penalty> penalties) => Penalties = penalties;
    }
}
