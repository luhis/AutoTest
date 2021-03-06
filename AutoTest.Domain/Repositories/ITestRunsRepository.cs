﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Domain.Repositories
{
    public interface ITestRunsRepository
    {
        Task<IEnumerable<TestRun>> GetAll(ulong eventId, CancellationToken cancellationToken);

        Task<IEnumerable<TestRun>> GetAll(ulong eventId, int ordinal, CancellationToken cancellationToken);

        Task AddTestRun(TestRun testRun, CancellationToken cancellationToken);

        Task UpdateTestRun(TestRun testRun, CancellationToken cancellationToken);
    }
}
