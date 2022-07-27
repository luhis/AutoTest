using AutoTest.Domain.Enums;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class SetEventStatus : IRequest
    {
        public SetEventStatus(ulong eventId, EventStatus status)
        {
            EventId = eventId;
            Status = status;
        }

        public ulong EventId { get; }
        public EventStatus Status { get; }
    }
}
