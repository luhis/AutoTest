using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class AddTestRunHandler : IRequestHandler<AddTestRun>
    {
        private readonly ITestRunsRepository testRunsRepository;
        private readonly ISignalRNotifier signalRNotifier;

        public AddTestRunHandler(ITestRunsRepository testRunsRepository, ISignalRNotifier signalRNotifier)
        {
            this.testRunsRepository = testRunsRepository;
            this.signalRNotifier = signalRNotifier;
        }

        async Task<Unit> IRequestHandler<AddTestRun, Unit>.Handle(AddTestRun request, CancellationToken cancellationToken)
        {
            await testRunsRepository.AddTestRun(request.TestRun, cancellationToken);
            await signalRNotifier.NewTestRun(request.TestRun.EventId, cancellationToken);
            return Unit.Value;
        }
    }
}
