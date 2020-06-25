using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Service.Handlers
{
    public class GetTestsHandler : IRequestHandler<GetTests, IEnumerable<Test>>
    {
        private readonly AutoTestContext autoTestContext;

        public GetTestsHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<IEnumerable<Test>> IRequestHandler<GetTests, IEnumerable<Test>>.Handle(GetTests request, CancellationToken cancellationToken)
        {
            return await this.autoTestContext.Tests.Where(b => b.EventId == request.EventId).ToArrayAsync(cancellationToken);
        }
    }
}
