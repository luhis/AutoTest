using System;
using System.Collections.Generic;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Service.Models
{
    public class EntrantTimes
    {
        public EntrantTimes(Entrant entrant, uint totalTime, IEnumerable<Tuple<uint, uint, uint>> times)
        {
            Entrant = entrant;
            TotalTime = totalTime;
            Times = times;
        }

        public Entrant Entrant { get; }

        public IEnumerable<Tuple<uint, uint, uint>> Times { get; }

        public uint TotalTime { get; }
    }
}
