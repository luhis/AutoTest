using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetMarshalHandler(IMarshalsRepository marshalsRepository) : IRequestHandler<GetMarshal, Marshal?>
    {
        Task<Marshal?> IRequestHandler<GetMarshal, Marshal?>.Handle(GetMarshal request, CancellationToken cancellationToken)
        {
            return marshalsRepository.GetById(request.EventId, request.MarshalId, cancellationToken)!;
        }
    }
}
