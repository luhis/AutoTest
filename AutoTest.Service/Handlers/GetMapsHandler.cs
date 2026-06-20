using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetMapsHandler(IFileRepository fileRepository) : IRequestHandler<GetMaps, string>
{
    public async ValueTask<string> Handle(GetMaps request, CancellationToken cancellationToken)
    {
        return await fileRepository.GetMaps(request.EventId, cancellationToken);
    }
}
