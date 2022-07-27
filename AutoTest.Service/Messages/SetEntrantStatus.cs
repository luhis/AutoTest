using AutoTest.Domain.Enums;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class SetEntrantStatus : IRequest
    {
        public SetEntrantStatus(ulong eventId, ulong entrantId, EntrantStatus status)
        {
            EventId = eventId;
            EntrantId = entrantId;
            Status = status;
        }

        public ulong EventId { get; }
        public ulong EntrantId { get; }
        public EntrantStatus Status { get; }
    }
}
