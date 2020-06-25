using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using MediatR;
namespace AutoTest.Service.Messages
{
    public class GetEntrants : IRequest<IEnumerable<Entrant>>
    {
        public GetEntrants(ulong eventId)
        {
            EventId = eventId;
        }

        public ulong EventId { get; }
    }
}
