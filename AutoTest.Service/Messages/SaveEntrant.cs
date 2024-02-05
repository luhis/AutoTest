using AutoTest.Domain.StorageModels;
using OneOf;
using OneOf.Types;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class SaveEntrant : IRequest<OneOf<Entrant, Error<string>>>
    {
        public SaveEntrant(Entrant entrant)
        {
            this.Entrant = entrant;
        }

        public Entrant Entrant { get; } // swap for tighter model without payment?
    }
}
