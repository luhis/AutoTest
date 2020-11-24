using MediatR;

namespace AutoTest.Service.Messages
{
    public class DeleteEntrant : IRequest
    {
        public DeleteEntrant(ulong entrantId)
        {
            EntrantId = entrantId;
        }

        public ulong EntrantId { get; }
    }
}
