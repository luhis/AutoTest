using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Persistence.Repositories
{
    public class TestRunsRepository : ITestRunsRepository
    {
        private readonly AutoTestContext _autoTestContext;

        public TestRunsRepository(AutoTestContext autoTestContext)
        {
            _autoTestContext = autoTestContext;
        }

        public Task<IEnumerable<TestRun>> GetAll(ulong eventId, CancellationToken cancellationToken)
        {
            return this._autoTestContext.TestRuns.Where(
                a => a.EventId == eventId).OrderBy(a => a.Created).ToEnumerableAsync(cancellationToken);
        }

        Task<IEnumerable<TestRun>> ITestRunsRepository.GetAll(ulong eventId, int ordinal, CancellationToken cancellationToken)
        {
            return this._autoTestContext.TestRuns.Where(
                a => a.EventId == eventId && a.Ordinal == ordinal).OrderBy(a => a.Created).ToEnumerableAsync(cancellationToken);
        }

        Task ITestRunsRepository.AddTestRun(TestRun testRun, CancellationToken cancellationToken)
        {
            this._autoTestContext.TestRuns.ThrowIfNull().Add(testRun);
            return this._autoTestContext.SaveChangesAsync(cancellationToken);
        }
    }
}
