using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetMarshalEventsHandler(IMarshalsRepository marshalsRepository) : IRequestHandler<GetMarshalEvents, IEnumerable<ulong>>
{
    public async ValueTask<IEnumerable<ulong>> Handle(GetMarshalEvents request, CancellationToken cancellationToken)
    {
        return await marshalsRepository.GetByEmail(request.EmailAddress).Select(a => a.EventId).Distinct().ToEnumerableAsync(cancellationToken);
    }
}
