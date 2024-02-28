using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;
using OneOf;
using OneOf.Types;

namespace AutoTest.Service.Handlers
{
    public class AddTestRunHandler(ITestRunsRepository testRunsRepository, IEventNotifier signalRNotifier, IMarshalsRepository marshalsRepository, IEventsRepository eventsRepository) : IRequestHandler<AddTestRun, OneOf<Success, Error<string>>>
    {
        async Task<OneOf<Success, Error<string>>> IRequestHandler<AddTestRun, OneOf<Success, Error<string>>>.Handle(AddTestRun request, CancellationToken cancellationToken)
        {
            var @event = await eventsRepository.GetById(request.EventId, cancellationToken);
            if (@event!.EventStatus != Domain.Enums.EventStatus.Running)
            {
                return new Error<string>("Event must be running to add Test Run");
            }
            var marshalId = await marshalsRepository.GetMarshalIdByEmail(request.EventId, request.EmailAddress, cancellationToken);
            var testRun = new TestRun(request.TestRunId, request.EventId, request.Ordinal, request.TimeInMS, request.EntrantId, request.Created, marshalId);
            testRun.SetPenalties(request.Penalties.ToArray());
            await testRunsRepository.AddTestRun(testRun, cancellationToken);
            await signalRNotifier.NewTestRun(testRun, cancellationToken);
            return new Success();
        }
    }
}
