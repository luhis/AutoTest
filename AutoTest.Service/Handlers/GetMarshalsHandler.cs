using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetMarshalsHandler : IRequestHandler<GetMarshals, IEnumerable<Marshal>>
    {
        private readonly AutoTestContext autoTestContext;

        public GetMarshalsHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        Task<IEnumerable<Marshal>> IRequestHandler<GetMarshals, IEnumerable<Marshal>>.Handle(GetMarshals request, CancellationToken cancellationToken)
        {
            return this.autoTestContext.Marshals!.Where(a => a.EventId == request.EventId).OrderByDescending(a => a.FamilyName).ToEnumerableAsync(cancellationToken);
        }
    }
}
