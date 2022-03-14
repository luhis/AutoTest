using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class MarkPaid : IRequest
    {
        public MarkPaid(ulong eventId, ulong entrantId, Payment? payment)
        {
            EventId = eventId;
            EntrantId = entrantId;
            Payment = payment;
        }

        public ulong EventId { get; }
        public ulong EntrantId { get; }
        public Payment? Payment { get; }
    }
}
