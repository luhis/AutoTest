using System;
using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class UpdateTestRun : IRequest
    {
        public UpdateTestRun(ulong testRunId, ulong eventId, int ordinal, int timeInMS, ulong entrantId, DateTime created, ulong marshalId, IEnumerable<Penalty> penalties)
        {
            TestRunId = testRunId;
            EventId = eventId;
            Ordinal = ordinal;
            TimeInMS = timeInMS;
            EntrantId = entrantId;
            Created = created;
            MarshalId = marshalId;
            Penalties = penalties;
        }
        public int Ordinal { get; set; }

        public ulong TestRunId { get; }

        public ulong EventId { get; }

        public int TimeInMS { get; }

        public DateTime Created { get; }

        public ulong EntrantId { get; }

        public ulong MarshalId { get; }

        public IEnumerable<Penalty> Penalties { get; }
    }
}
