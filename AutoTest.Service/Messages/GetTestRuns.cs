using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class GetTestRuns : IRequest<IEnumerable<TestRun>>
    {
        public GetTestRuns(ulong eventId, int ordinal)
        {
            EventId = eventId;
            Ordinal = ordinal;
        }

        public int Ordinal { get; }

        public ulong EventId { get; }
    }
}
