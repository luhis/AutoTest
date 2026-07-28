using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetRegsHandler(IFileRepository fileRepository) : IRequestHandler<GetRegs, string>
{
    public ValueTask<string> Handle(GetRegs request, CancellationToken cancellationToken) =>
        new(fileRepository.GetRegs(request.EventId, cancellationToken));
}
