using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetTestRunsHandler : IRequestHandler<GetTestRuns, IEnumerable<TestRun>>
    {
        private readonly ITestRunsRepository testRunsRepository;

        public GetTestRunsHandler(ITestRunsRepository testRunsRepository)
        {
            this.testRunsRepository = testRunsRepository;
        }

        Task<IEnumerable<TestRun>> IRequestHandler<GetTestRuns, IEnumerable<TestRun>>.Handle(GetTestRuns request, CancellationToken cancellationToken)
        {
            return testRunsRepository.GetAll(request.EventId, request.Ordinal, cancellationToken);
        }
    }
}
