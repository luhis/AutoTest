using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetTestRunsHandler(ITestRunsRepository testRunsRepository) : IRequestHandler<GetTestRuns, IEnumerable<TestRun>>
{
    public ValueTask<IEnumerable<TestRun>> Handle(GetTestRuns request, CancellationToken cancellationToken) =>
        new(testRunsRepository.GetAll(request.EventId, request.Ordinal, cancellationToken));
}
