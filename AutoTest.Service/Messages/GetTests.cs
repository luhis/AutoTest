using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class GetTests : IRequest<IEnumerable<Course>>
    {
        public GetTests(ulong eventId)
        {
            EventId = eventId;
        }

        public ulong EventId { get; }
    }
}
