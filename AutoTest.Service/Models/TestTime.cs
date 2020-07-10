using System.Collections.Generic;

namespace AutoTest.Service.Models
{
    public class TestTime
    {
        public TestTime(int ordinal, IEnumerable<int> timesInMs)
        {
            Ordinal = ordinal;
            TimesInMs = timesInMs;
        }

        public int Ordinal { get; }

        public IEnumerable<int> TimesInMs { get; }
    }
}
