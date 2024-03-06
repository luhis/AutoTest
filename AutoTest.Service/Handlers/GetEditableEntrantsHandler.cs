using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetEditableEntrantsHandler(AutoTestContext autoTestContext) : IRequestHandler<GetEditableEntrants, IEnumerable<ulong>>
    {
        Task<IEnumerable<ulong>> IRequestHandler<GetEditableEntrants, IEnumerable<ulong>>.Handle(GetEditableEntrants request, CancellationToken cancellationToken)
        {
            return autoTestContext.Entrants.Where(a => a.Email == request.EmailAddress).Select(a => a.EntrantId).ToEnumerableAsync(cancellationToken);
        }
    }
}
