using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Persistence.Repositories
{
    public class TestRunsRepository(AutoTestContext autoTestContext) : ITestRunsRepository
    {
        public Task<IEnumerable<TestRun>> GetAll(ulong eventId, CancellationToken cancellationToken)
        {
            return autoTestContext.TestRuns.Where(
                a => a.EventId == eventId).OrderBy(a => a.Created).ToEnumerableAsync(cancellationToken);
        }

        Task<IEnumerable<TestRun>> ITestRunsRepository.GetAll(ulong eventId, int ordinal, CancellationToken cancellationToken)
        {
            return autoTestContext.TestRuns.Where(
                a => a.EventId == eventId && a.Ordinal == ordinal).OrderBy(a => a.Created).ToEnumerableAsync(cancellationToken);
        }

        Task ITestRunsRepository.AddTestRun(TestRun testRun, CancellationToken cancellationToken)
        {
            autoTestContext.TestRuns.Add(testRun);
            return autoTestContext.SaveChangesAsync(cancellationToken);
        }

        Task ITestRunsRepository.UpdateTestRun(TestRun testRun, CancellationToken cancellationToken)
        {
            autoTestContext.TestRuns.Update(testRun);
            return autoTestContext.SaveChangesAsync(cancellationToken);
        }
    }
}
