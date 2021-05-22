using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Service.Handlers
{
    public class AddTestRunHandler : IRequestHandler<AddTestRun>
    {
        private readonly ITestRunsRepository testRunsRepository;
        private readonly AutoTestContext _autoTestContext;
        private readonly ISignalRNotifier signalRNotifier;

        public AddTestRunHandler(ITestRunsRepository testRunsRepository, ISignalRNotifier signalRNotifier, AutoTestContext autoTestContext)
        {
            this.testRunsRepository = testRunsRepository;
            this.signalRNotifier = signalRNotifier;
            _autoTestContext = autoTestContext;
        }

        async Task<Unit> IRequestHandler<AddTestRun, Unit>.Handle(AddTestRun request, CancellationToken cancellationToken)
        {
            var marshal = await _autoTestContext.Marshals!.SingleAsync(a => a.Email == request.EmailAddress, cancellationToken);
            var testRun = new TestRun(request.TestRunId, request.EventId, request.Ordinal, request.TimeInMS, request.EntrantId, request.Created, marshal.MarshalId);
            testRun.SetPenalties(request.Penalties.ToArray());
            await testRunsRepository.AddTestRun(testRun, cancellationToken);
            await signalRNotifier.NewTestRun(testRun, cancellationToken);
            return Unit.Value;
        }
    }
}
