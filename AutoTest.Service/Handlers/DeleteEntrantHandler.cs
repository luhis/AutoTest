using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class DeleteEntrantHandler(IEntrantsRepository entrantsRepository) : IRequestHandler<DeleteEntrant>
{
    public async ValueTask<Unit> Handle(DeleteEntrant request, CancellationToken cancellationToken)
    {
        var found = await entrantsRepository.GetById(request.EventId, request.EntrantId, cancellationToken);
        if (found is null)
        {
            throw new NullReferenceException();
        }
        await entrantsRepository.Delete(found, cancellationToken);
        return Unit.Value;
    }
}
