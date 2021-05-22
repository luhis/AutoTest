using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class UpdateTestRunHandler : IRequestHandler<UpdateTestRun>
    {
        private readonly ITestRunsRepository testRunsRepository;
        private readonly ISignalRNotifier signalRNotifier;

        public UpdateTestRunHandler(ITestRunsRepository testRunsRepository, ISignalRNotifier signalRNotifier)
        {
            this.testRunsRepository = testRunsRepository;
            this.signalRNotifier = signalRNotifier;
        }

        async Task<Unit> IRequestHandler<UpdateTestRun, Unit>.Handle(UpdateTestRun request, CancellationToken cancellationToken)
        {
            var testRun = new TestRun(request.TestRunId, request.EventId, request.Ordinal, request.TimeInMS, request.EntrantId, request.Created, request.MarshalId);
            testRun.SetPenalties(request.Penalties.ToArray());
            await testRunsRepository.UpdateTestRun(testRun, cancellationToken);
            await signalRNotifier.NewTestRun(testRun, cancellationToken);
            return Unit.Value;
        }
    }
}
