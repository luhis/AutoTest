using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class AddTestRun : IRequest
    {
        public AddTestRun(TestRun testRun)
        {
            TestRun = testRun;
        }

        public TestRun TestRun { get; }
    }
}
