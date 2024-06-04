using AutoTest.Service.Models;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class GetAwards : IRequest<Awards>
    {
        public GetAwards(ulong eventId)
        {
            EventId = eventId;
        }

        public ulong EventId { get; }
    }
}
