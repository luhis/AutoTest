using System;
using System.Collections.Generic;

namespace AutoTest.Domain.StorageModels
{
    public class TestRun(ulong testRunId, ulong eventId, int ordinal, int timeInMS, ulong entrantId, DateTime created, ulong marshalId)
    {
        public int Ordinal { get; set; } = ordinal;

        public ulong TestRunId { get; } = testRunId;

        public ulong EventId { get; } = eventId;

        public int TimeInMS { get; } = timeInMS;

        public DateTime Created { get; } = created;

        public ulong EntrantId { get; } = entrantId;

        public ulong MarshalId { get; } = marshalId;

        public ICollection<Penalty> Penalties { get; private set; } = new List<Penalty>();

        public void SetPenalties(ICollection<Penalty> penalties) => Penalties = penalties;
    }
}
