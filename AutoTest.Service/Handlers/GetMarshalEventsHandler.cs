using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetMarshalEventsHandler : IRequestHandler<GetMarshalEvents, IEnumerable<ulong>>
    {
        private readonly IMarshalsRepository marshalsRepository;

        public GetMarshalEventsHandler(IMarshalsRepository marshalsRepository)
        {
            this.marshalsRepository = marshalsRepository;
        }

        Task<IEnumerable<ulong>> IRequestHandler<GetMarshalEvents, IEnumerable<ulong>>.Handle(GetMarshalEvents request, CancellationToken cancellationToken)
        {
            return this.marshalsRepository.GetByEmail(request.EmailAddress).Select(a => a.EventId).Distinct().ToEnumerableAsync(cancellationToken);
        }
    }
}
