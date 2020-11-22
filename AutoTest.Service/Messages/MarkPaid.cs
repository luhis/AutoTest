using MediatR;

namespace AutoTest.Service.Messages
{
    public class MarkPaid : IRequest
    {
        public MarkPaid(ulong entrantId, bool isPaid)
        {
            EntrantId = entrantId;
            IsPaid = isPaid;
        }

        public ulong EntrantId { get; }
        public bool IsPaid { get; }
    }
}
