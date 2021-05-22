using System;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class AddTestRun : IRequest
    {
        public AddTestRun(ulong testRunId, ulong eventId, int ordinal, int timeInMS, ulong entrantId, DateTime created, string emailAddress)
        {
            TestRunId = testRunId;
            EventId = eventId;
            Ordinal = ordinal;
            TimeInMS = timeInMS;
            EntrantId = entrantId;
            Created = created;
            EmailAddress = emailAddress;
        }
        public int Ordinal { get; set; }

        public ulong TestRunId { get; }

        public ulong EventId { get; }

        public int TimeInMS { get; }

        public DateTime Created { get; }

        public ulong EntrantId { get; }

        public string EmailAddress { get; }
    }
}
