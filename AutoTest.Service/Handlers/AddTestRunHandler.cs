using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class AddTestRunHandler : IRequestHandler<AddTestRun>
    {
        private readonly ITestRunsRepository testRunsRepository;

        public AddTestRunHandler(ITestRunsRepository testRunsRepository)
        {
            this.testRunsRepository = testRunsRepository;
        }

        async Task<Unit> IRequestHandler<AddTestRun, Unit>.Handle(AddTestRun request, CancellationToken cancellationToken)
        {
            await testRunsRepository.AddTestRun(request.TestRun, cancellationToken);
            return Unit.Value;
        }
    }
}
