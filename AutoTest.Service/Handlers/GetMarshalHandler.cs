using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetMarshalHandler : IRequestHandler<GetMarshal, Marshal?>
    {
        private readonly IMarshalsRepository marshalsRepository;

        public GetMarshalHandler(IMarshalsRepository marshalsRepository)
        {
            this.marshalsRepository = marshalsRepository;
        }

        Task<Marshal?> IRequestHandler<GetMarshal, Marshal?>.Handle(GetMarshal request, CancellationToken cancellationToken)
        {
            return this.marshalsRepository.GetById(request.EventId, request.MarshalId, cancellationToken)!;
        }
    }
}
