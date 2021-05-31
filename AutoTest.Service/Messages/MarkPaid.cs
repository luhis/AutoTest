using MediatR;

namespace AutoTest.Service.Messages
{
    public class MarkPaid : IRequest
    {
        public MarkPaid(ulong eventId, ulong entrantId, bool isPaid)
        {
            EventId = eventId;
            EntrantId = entrantId;
            IsPaid = isPaid;
        }

        public ulong EventId { get; }
        public ulong EntrantId { get; }
        public bool IsPaid { get; }
    }
}
