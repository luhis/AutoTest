using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class UpdateTestRunHandler(ITestRunsRepository testRunsRepository, IEventNotifier signalRNotifier) : IRequestHandler<UpdateTestRun>
{
    public async ValueTask<Unit> Handle(UpdateTestRun request, CancellationToken cancellationToken)
    {
        var testRun = new TestRun(request.TestRunId, request.EventId, request.Ordinal, request.TimeInMS, request.EntrantId, request.Created, request.MarshalId);
        testRun.SetPenalties(request.Penalties.ToArray());
        await testRunsRepository.UpdateTestRun(testRun, cancellationToken);
        await signalRNotifier.NewTestRun(testRun, cancellationToken);
        return Unit.Value;
    }
}
