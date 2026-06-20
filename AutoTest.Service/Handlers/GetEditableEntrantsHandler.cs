using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetEditableEntrantsHandler(AutoTestContext autoTestContext) : IRequestHandler<GetEditableEntrants, IEnumerable<ulong>>
{
    public async ValueTask<IEnumerable<ulong>> Handle(GetEditableEntrants request, CancellationToken cancellationToken)
    {
        return await autoTestContext.Entrants.Where(a => a.Email == request.EmailAddress).Select(a => a.EntrantId).ToEnumerableAsync(cancellationToken);
    }
}
