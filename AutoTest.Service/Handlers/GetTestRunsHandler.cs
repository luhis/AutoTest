using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetTestRunsHandler(ITestRunsRepository testRunsRepository) : IRequestHandler<GetTestRuns, IEnumerable<TestRun>>
    {
        Task<IEnumerable<TestRun>> IRequestHandler<GetTestRuns, IEnumerable<TestRun>>.Handle(GetTestRuns request, CancellationToken cancellationToken)
        {
            return testRunsRepository.GetAll(request.EventId, request.Ordinal, cancellationToken);
        }
    }
}
