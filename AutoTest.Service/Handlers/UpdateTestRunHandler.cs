using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
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
            await testRunsRepository.UpdateTestRun(request.TestRun, cancellationToken);
            await signalRNotifier.NewTestRun(request.TestRun, cancellationToken);
            return Unit.Value;
        }
    }
}
