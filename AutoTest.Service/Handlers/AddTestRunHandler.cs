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
    public class AddTestRunHandler : IRequestHandler<AddTestRun>
    {
        private readonly ITestRunsRepository testRunsRepository;
        private readonly IEventsRepository _eventsRepository;
        private readonly IMarshalsRepository _marshalsRepository;
        private readonly IEventNotifier signalRNotifier;

        public AddTestRunHandler(ITestRunsRepository testRunsRepository, IEventNotifier signalRNotifier, IMarshalsRepository marshalsRepository, IEventsRepository eventsRepository)
        {
            this.testRunsRepository = testRunsRepository;
            this.signalRNotifier = signalRNotifier;
            _marshalsRepository = marshalsRepository;
            _eventsRepository = eventsRepository;
        }

        async Task<Unit> IRequestHandler<AddTestRun, Unit>.Handle(AddTestRun request, CancellationToken cancellationToken)
        {
            var @event = await _eventsRepository.GetById(request.EventId, cancellationToken);
            if (@event.EventStatus != Domain.Enums.EventStatus.Running)
            {
                throw new System.Exception("Event must be running to add Test Run");
            }
            var marshalId = await _marshalsRepository.GetMashalIdByEmail(request.EventId, request.EmailAddress, cancellationToken);
            var testRun = new TestRun(request.TestRunId, request.EventId, request.Ordinal, request.TimeInMS, request.EntrantId, request.Created, marshalId);
            testRun.SetPenalties(request.Penalties.ToArray());
            await testRunsRepository.AddTestRun(testRun, cancellationToken);
            await signalRNotifier.NewTestRun(testRun, cancellationToken);
            return Unit.Value;
        }
    }
}
