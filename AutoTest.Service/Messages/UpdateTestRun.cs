using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class UpdateTestRun : IRequest
    {
        public UpdateTestRun(TestRun testRun)
        {
            TestRun = testRun;
        }

        public TestRun TestRun { get; }
    }
}
