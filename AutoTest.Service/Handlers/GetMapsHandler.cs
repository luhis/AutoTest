using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetMapsHandler(IFileRepository fileRepository) : IRequestHandler<GetMaps, string>
    {
        Task<string> IRequestHandler<GetMaps, string>.Handle(GetMaps request, CancellationToken cancellationToken)
        {
            return fileRepository.GetMaps(request.EventId, cancellationToken);
        }
    }
}
