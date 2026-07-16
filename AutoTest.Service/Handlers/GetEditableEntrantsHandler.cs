using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetEditableEntrantsHandler(IEntrantsRepository entrantsRepository) : IRequestHandler<GetEditableEntrants, IEnumerable<ulong>>
{
    public async ValueTask<IEnumerable<ulong>> Handle(GetEditableEntrants request, CancellationToken cancellationToken)
    {
        return await entrantsRepository.GetEntrantIdsByEmail(request.EmailAddress, cancellationToken);
    }
}
