using System;
using System.Collections.Generic;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Service.Models
{
    public class EntrantTimes
    {
        public EntrantTimes(Entrant entrant, int totalTime, IEnumerable<TestTime> times)
        {
            Entrant = entrant;
            TotalTime = totalTime;
            Times = times;
        }

        public Entrant Entrant { get; }

        public IEnumerable<TestTime> Times { get; }

        public int TotalTime { get; }
    }
}
