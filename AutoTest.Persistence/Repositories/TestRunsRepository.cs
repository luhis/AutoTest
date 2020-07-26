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

        Task<IEnumerable<TestRun>> ITestRunsRepository.GetAll(ulong testId, CancellationToken cancellationToken)
        {
            return this._autoTestContext.TestRuns.Where(
                a => a.TestId == testId).OrderBy(a => a.Created).ToEnumerableAsync(cancellationToken);
        }

        Task ITestRunsRepository.AddTestRun(TestRun testRun, CancellationToken cancellationToken)
        {
            this._autoTestContext.TestRuns.ThrowIfNull().Add(testRun);
            return this._autoTestContext.SaveChangesAsync(cancellationToken);
        }
    }
}
