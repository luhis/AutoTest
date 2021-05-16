using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetMarshalEventsHandler : IRequestHandler<GetMarshalEvents, IEnumerable<ulong>>
    {
        private readonly AutoTestContext autoTestContext;

        public GetMarshalEventsHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        Task<IEnumerable<ulong>> IRequestHandler<GetMarshalEvents, IEnumerable<ulong>>.Handle(GetMarshalEvents request, CancellationToken cancellationToken)
        {
            return this.autoTestContext.Marshals.Where(a => a.Email == request.EmailAddress).Select(a => a.EventId).Distinct().ToEnumerableAsync(cancellationToken);
        }
    }
}
