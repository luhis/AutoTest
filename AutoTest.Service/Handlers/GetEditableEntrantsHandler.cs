using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetEditableEntrantsHandler : IRequestHandler<GetEditableEntrants, IEnumerable<ulong>>
    {
        private readonly AutoTestContext autoTestContext;

        public GetEditableEntrantsHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        Task<IEnumerable<ulong>> IRequestHandler<GetEditableEntrants, IEnumerable<ulong>>.Handle(GetEditableEntrants request, CancellationToken cancellationToken)
        {
            return this.autoTestContext.Entrants!.Where(a => a.Email == request.EmailAddress).Select(a => a.EntrantId).ToEnumerableAsync(cancellationToken);
        }
    }
}
