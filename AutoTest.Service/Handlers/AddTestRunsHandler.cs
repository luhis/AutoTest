using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class AddTestRunsHandler : IRequestHandler<AddTestRun>
    {
        private readonly AutoTestContext autoTestContext;

        public AddTestRunsHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<Unit> IRequestHandler<AddTestRun, Unit>.Handle(AddTestRun request, CancellationToken cancellationToken)
        {
            this.autoTestContext.TestRuns.Add(request.TestRun);
            await this.autoTestContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
