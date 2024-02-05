using System;
using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using MediatR;
using OneOf;
using OneOf.Types;

namespace AutoTest.Service.Messages
{
    public class AddTestRun : IRequest<OneOf<Success, Error<string>>>
    {
        public AddTestRun(ulong testRunId, ulong eventId, int ordinal, int timeInMS, ulong entrantId, DateTime created, string emailAddress, IEnumerable<Penalty> penalties)
        {
            TestRunId = testRunId;
            EventId = eventId;
            Ordinal = ordinal;
            TimeInMS = timeInMS;
            EntrantId = entrantId;
            Created = created;
            EmailAddress = emailAddress;
            Penalties = penalties;
        }
        public int Ordinal { get; set; }

        public ulong TestRunId { get; }

        public ulong EventId { get; }

        public int TimeInMS { get; }

        public DateTime Created { get; }

        public ulong EntrantId { get; }

        public string EmailAddress { get; }

        public IEnumerable<Penalty> Penalties { get; }
    }
}
