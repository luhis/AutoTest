using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class GetTestRuns : IRequest<IEnumerable<TestRun>>
    {
        public GetTestRuns(ulong testId)
        {
            TestId = testId;
        }

        public ulong TestId { get; }
    }
}
