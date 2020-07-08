using System.Collections.Generic;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Service.ResultCalculation
{
    public interface ITotalTimeCalculator
    {
        int GetTotalTime(IEnumerable<TestRun> testRuns, IEnumerable<TestRun> allTestRuns);
    }
}
