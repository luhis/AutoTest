using System.Collections.Generic;
using AutoTest.Service.Models;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class GetResults : IRequest<IEnumerable<Result>>
    {
        public GetResults(ulong eventId)
        {
            EventId = eventId;
        }

        public ulong EventId { get; }
    }
}
