using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class GetTestRuns : IRequest<IEnumerable<TestRun>>
    {
        public GetTestRuns(ulong eventId)
        {
            EventId = eventId;
        }

        public ulong EventId { get; }
    }
}
