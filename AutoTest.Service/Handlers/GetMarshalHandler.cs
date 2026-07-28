using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetMarshalHandler(IMarshalsRepository marshalsRepository) : IRequestHandler<GetMarshal, Marshal?>
{
    public ValueTask<Marshal?> Handle(GetMarshal request, CancellationToken cancellationToken) =>
        new(marshalsRepository.GetById(request.EventId, request.MarshalId, cancellationToken));
}
