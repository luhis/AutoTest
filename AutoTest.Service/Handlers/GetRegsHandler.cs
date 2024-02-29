using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetRegsHandler(IFileRepository fileRepository) : IRequestHandler<GetRegs, string>
    {
        Task<string> IRequestHandler<GetRegs, string>.Handle(GetRegs request, CancellationToken cancellationToken)
        {
            return fileRepository.GetRegs(request.EventId, cancellationToken);
        }
    }
}
