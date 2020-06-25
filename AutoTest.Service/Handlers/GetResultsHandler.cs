using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using AutoTest.Service.Models;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetResultsHandler : IRequestHandler<GetResults, IEnumerable<Result>>
    {
        private readonly AutoTestContext autoTestContext;

        public GetResultsHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        Task<IEnumerable<Result>> IRequestHandler<GetResults, IEnumerable<Result>>.Handle(GetResults request, CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<Result>>(new[] { new Result() });
        }
    }
}
