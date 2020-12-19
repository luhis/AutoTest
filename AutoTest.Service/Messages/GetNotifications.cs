using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class GetNotifications : IRequest<IEnumerable<Notification>>
    {
        public GetNotifications(ulong eventId)
        {
            EventId = eventId;
        }

        public ulong EventId { get; }
    }
}
