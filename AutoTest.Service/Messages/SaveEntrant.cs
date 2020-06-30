using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class SaveEntrant : IRequest<ulong>
    {
        public SaveEntrant(Entrant entrant)
        {
            this.Entrant = entrant;
        }

        public Entrant Entrant { get; }
    }
}
