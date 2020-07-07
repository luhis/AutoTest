using System.Collections.Generic;

namespace AutoTest.Service.Models
{
    public class TestTime
    {
        public TestTime(uint ordinal, IEnumerable<ulong> timesInMs)
        {
            Ordinal = ordinal;
            TimesInMs = timesInMs;
        }

        public uint Ordinal { get; }

        public IEnumerable<ulong> TimesInMs { get; }
    }
}
