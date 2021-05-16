using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using MediatR;
namespace AutoTest.Service.Messages
{
    public class GetMarshals : IRequest<IEnumerable<Marshal>>
    {
        public GetMarshals(ulong eventId)
        {
            EventId = eventId;
        }

        public ulong EventId { get; }
    }
}
