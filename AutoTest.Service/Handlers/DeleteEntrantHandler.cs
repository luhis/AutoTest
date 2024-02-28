using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class DeleteEntrantHandler(IEntrantsRepository entrantsRepository) : IRequestHandler<DeleteEntrant>
    {
        async Task IRequestHandler<DeleteEntrant>.Handle(DeleteEntrant request, CancellationToken cancellationToken)
        {
            var found = await entrantsRepository.GetById(request.EventId, request.EntrantId, cancellationToken);
            if (found == null)
            {
                throw new NullReferenceException();
            }
            await entrantsRepository.Delete(found, cancellationToken);
        }
    }
}
