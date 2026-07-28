using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetEditableMarshalsHandler(IMarshalsRepository marshalsRepository) : IRequestHandler<GetEditableMarshals, IEnumerable<ulong>>
{
    public ValueTask<IEnumerable<ulong>> Handle(GetEditableMarshals request, CancellationToken cancellationToken) =>
        new(marshalsRepository.GetByEmail(request.EmailAddress).Select(a => a.MarshalId).ToEnumerableAsync(cancellationToken));
}
