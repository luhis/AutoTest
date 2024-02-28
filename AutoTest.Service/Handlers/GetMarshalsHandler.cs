using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetMarshalsHandler(IMarshalsRepository marshalsRepository) : IRequestHandler<GetMarshals, IEnumerable<Marshal>>
    {
        async Task<IEnumerable<Marshal>> IRequestHandler<GetMarshals, IEnumerable<Marshal>>.Handle(GetMarshals request, CancellationToken cancellationToken)
        {
            var partial = await marshalsRepository.GetByEventId(request.EventId).OrderByDescending(a => a.FamilyName).ToEnumerableAsync(cancellationToken);
            return partial.OrderBy(a => a.FamilyName).ThenBy(a => a.GivenName);
        }
    }
}
