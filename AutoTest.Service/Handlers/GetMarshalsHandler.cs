using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetMarshalsHandler(IMarshalsRepository marshalsRepository) : IRequestHandler<GetMarshals, IEnumerable<Marshal>>
{
    public async ValueTask<IEnumerable<Marshal>> Handle(GetMarshals request, CancellationToken cancellationToken)
    {
        // in efCore 10, the migrations will not create server side indexes, will fix this in future
        var marshals = await marshalsRepository.GetByEventId(request.EventId).OrderBy(a => a.FamilyName).ToEnumerableAsync(cancellationToken);
        return marshals.OrderBy(a => a.FamilyName).ThenBy(a => a.GivenName);
    }
}
