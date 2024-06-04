// Ignore Spelling: Ftd

using System.Collections.Generic;

namespace AutoTest.Service.Models
{
    public class Awards
    {
        public Awards(EntrantTimes ftd, IReadOnlyList<Result> classAwards)
        {
            Ftd = ftd;
            ClassAwards = classAwards;
        }

        public EntrantTimes Ftd { get; }

        public IReadOnlyList<Result> ClassAwards { get; }
    }
}
