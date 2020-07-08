using System.Collections.Generic;

namespace AutoTest.Service.Models
{
    public class Result
    {
        public Result(string @class, IEnumerable<EntrantTimes> entrantTimes)
        {
            Class = @class;
            EntrantTimes = entrantTimes;
        }

        public string Class { get; }

        public IEnumerable<EntrantTimes> EntrantTimes { get; }
    }
}
