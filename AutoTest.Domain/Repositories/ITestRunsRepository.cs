using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Domain.Repositories
{
    public interface ITestRunsRepository
    {
        Task<IEnumerable<TestRun>> GetAll(ulong testId, CancellationToken cancellationToken);

        Task AddTestRun(TestRun testRun, CancellationToken cancellationToken);
    }
}
